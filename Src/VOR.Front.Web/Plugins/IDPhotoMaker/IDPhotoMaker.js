; (function (window, document) {

    var Fabric = {
        Canvas: null,
        CropZone: null
    };

    var defaults = {
        lang: 'fr',
        imageExtensions: ["jpg", "jpeg", "jpe", "jfif", "png", "gif", "bmp"],
        imageMaxSize: 5, //Mo
        imageColorRequired: true,
        minWidth: null,
        minHeight: null,
        maxWidth: 700,
        maxHeight: 400,
        crop: {
            width: 100,
            height: 150,
            ratio: 1.5
        },
        scale: 1.0,
        scaleFactor: 0.99,
        save: function (data) { },
        error: function (message) { },
        exit: function () { },
        outputImageFormat: 'jpeg',
        outputImageQuality: 0.8
    };

    IDPhotoMaker = (function () {

        return {

            init: function (container, options) {
                var _this = this;
                this.container = $(container);
                this.container.hide();
                this.options = $.extend({}, defaults, options);
                this.options.lang = this.options.lang.split('-')[0];

                this.loadTemplate(function () {
                    _this.container.show();
                    _this.initFabricUtil();
                    _this.initEvents();
                });
            },

            initEvents: function () {
                var _this = this;

                /******* Accueil *******/

                //glisser la photo
                $("#IDPhotoMaker-ui-home-droparea").on("dragover", function (e) {
                    $(this).addClass("IDPhotoMaker-ui-home-droparea-hover");
                    e.preventDefault();
                    return false;
                });
                $("#IDPhotoMaker-ui-home-droparea").on("dragleave", function (e) {
                    $(this).removeClass("IDPhotoMaker-ui-home-droparea-hover");
                    e.preventDefault();
                    return false;
                });
                $("#IDPhotoMaker-ui-home-droparea").on('drop', function (e) {
                    e.preventDefault();

                    $(this).removeClass("IDPhotoMaker-ui-home-droparea-hover");

                    var files = e.originalEvent.target.files || e.originalEvent.dataTransfer.files;

                    _this.readFile(files);

                    return false;
                });

                //afficher la camera
                $("#home_btn_camera").click(function () {
                    $("#IDPhotoMaker-ui-home").hide();
                    $("#IDPhotoMaker-ui-camera").show();
                    _this.startWebcam();
                });

                //importer la photo
                $("#home_btn_upload").click(function () {
                    $("#home_fileupload").click();
                });

                $("#home_fileupload").on("change", function (e) {
                    var fileUpload = $(this);
                    if (fileUpload.val()) {
                        var files = e.originalEvent.target.files || e.originalEvent.dataTransfer.files;
                        _this.readFile(files);
                        setTimeout(function () {
                            fileUpload.val("");
                        }, 1000);
                    }
                });

                //quitter
                $("#home_btn_exit").click(function () {
                    _this.dispose();
                    _this.options.exit();
                });

                /******* Camera *******/

                //démarrer la camera
                $("#camera_btn_start").click(function () {
                    _this.startWebcam();
                });

                //arrêter la camera
                $("#camera_btn_stop").click(function () {
                    _this.stopWebcam();
                });

                //prendre une photo
                $("#camera_btn_capture").click(function () {
                    Webcam.snap(
                        function (data) {
                            _this.tempData = data;
                            _this.initImageEditor(data);
                            $("#IDPhotoMaker-ui-camera").hide();
                            $("#IDPhotoMaker-ui-editor").show();
                            _this.stopWebcam();
                        },
                        function (error) {
                            _this.options.error(error);
                        }
                    );
                });

                //retour à l'écran d'accueil
                $("#camera_btn_cancel").click(function () {
                    _this.stopWebcam();
                    $("#IDPhotoMaker-ui-home").show();
                    $("#IDPhotoMaker-ui-camera").hide();
                });


                /*******  Image editor ********/

                //zoom in :
                //maintenir le bouton
                $("#btn_zoomIn").mousedown(function () {
                    _this.zoomInterval = setInterval(function () {
                        _this.zoom(1 / _this.options.scaleFactor);
                    }, 30);
                });
                //lacher le bouton
                $("#btn_zoomIn").mouseup(function () {
                    clearInterval(_this.zoomInterval);
                });

                //zoom out :
                //maintenir le bouton
                $("#btn_zoomOut").mousedown(function () {
                    _this.zoomInterval = setInterval(function () {
                        _this.zoom(_this.options.scaleFactor);
                    }, 30);
                });
                //lacher le bouton
                $("#btn_zoomOut").mouseup(function () {
                    clearInterval(_this.zoomInterval);
                });

                //rotate right
                $("#btn_rotateR").click(function () {
                    _this.rotate(90);
                });

                //rotate left
                $("#btn_rotateL").click(function () {
                    _this.rotate(-90);
                });

                //flip H
                $("#btn_flipH").click(function () {
                    _this.flipH();
                });

                //flip V
                $("#btn_flipV").click(function () {
                    _this.flipV();
                });

                //sélectionner une nouevelle photo
                $("#btn_newPhoto").click(function () {
                    _this.selfDestroy();
                    $("#IDPhotoMaker-ui-home").show();
                    $("#IDPhotoMaker-ui-editor").hide();
                });

                //save
                $("#btn_save").click(function () {
                    $("#IDPhotoMaker-ui-home").show();
                    $("#IDPhotoMaker-ui-editor").hide();

                    _this.cropCurrentZone(function () {
                        var result = _this.snapshotImage();
                        _this.options.save(result);
                        _this.selfDestroy();
                    });
                });
            },

            initDOM: function () {

                var element = document.querySelector("#canvas_container");

                element.innerHTML = "";

                var canvas = document.createElement('canvas');
                canvas.width = 700;
                canvas.height = 360;
                var canvasContainer = document.createElement('div');
                canvasContainer.appendChild(canvas);

                var container = document.createElement('div');

                container.appendChild(canvasContainer);

                element.appendChild(container);

                var bgImg = new Image();
                bgImg.src = "data:image/gif;base64,R0lGODlhCgAKAIAAAOLi4v///yH5BAAHAP8ALAAAAAAKAAoAAAIRhB2ZhxoM3GMSykqd1VltzxQAOw==";

                this.canvas = new Fabric.Canvas(canvas, {
                    selection: false
                });

                this.canvas.backgroundColor = new fabric.Pattern({ source: bgImg })
                this.canvas.add(this.image);
                this.canvas.on('object:moving', this.onObjectMoving.bind(this));
                this.canvas.on('object:scaling', this.onObjectScaling.bind(this));
            },

            initImageEditor: function (imgData) {
                var _this = this;

                var element = document.querySelector("#canvas_container");

                var image = new Image();

                image.onload = function () {

                    if (!$(this).hasClass("canvas-img")) {
                        if (this.width < _this.options.crop.width || this.height < _this.options.crop.height) {
                            _this.options.error(_this.strings.editor.msg_photo_small);
                        } else {

                            if (_this.options.imageColorRequired && _this.isImageGrayScale(this)) {
                                _this.options.error(_this.strings.editor.msg_photo_color_required);
                                return;
                            }

                            $("#IDPhotoMaker-ui-home").hide();
                            $("#IDPhotoMaker-ui-editor").show();

                            _this.image = _this.createFabricImage(this);
                            _this.initDOM(element);
                            _this.selectZone(10, 10, _this.options.crop.width, _this.options.crop.height);
                        }
                    }
                }

                image.src = imgData;

                _this.data = imgData;
            },

            loadTemplate: function (callback) {
                var _this = this;
                var baseUrl = getBaseUrl();
                var tmplUrl = "{0}/template.html?_={1}".format(baseUrl, getTimestamp());
                var resourceUrl = "{0}/resources/strings.{1}.js?_={2}".format(baseUrl, _this.options.lang, getTimestamp());

                loadScript("{0}/lib/jquery.tmpl.min.js".format(baseUrl), function () {
                    $.get(tmplUrl, function (html) {
                        $.getJSON(resourceUrl, function (strings) {
                            _this.strings = strings;

                            var data = {
                                baseUrl: baseUrl,
                                timestamp: getTimestamp(),
                                strings: strings,
                                imageExtensions: _this.options.imageExtensions,
                                imageColorRequired: _this.options.imageColorRequired
                            };

                            _this.container.html($.tmpl(html, data));

                            callback();
                        });
                    });
                });
            },

            readFile: function (files) {
                var _this = this;

                if (files.length == 0 || (files.length != 0 && !_this.isValidImage(files[0]))) {

                    if($("#IDPhotoMaker-ui-home-droparea").length > 0)
                        $("#IDPhotoMaker-ui-home-droparea").removeClass("IDPhotoMaker-ui-home-droparea-hover");

                    _this.options.error(_this.strings.editor.msg_photo_invalid);

                    return false;
                }

                var file = files[0];

                if (!_this.validateImageSize(file.size)) {
                    _this.options.error(_this.strings.editor.msg_photo_maxSize.format(_this.options.imageMaxSize));
                    return false;
                }

                var reader = new FileReader();

                reader.onload = function (event) {
                    _this.tempData = event.target.result;
                    _this.initImageEditor(event.target.result);
                }

                reader.readAsDataURL(file);
            },

            isValidImage: function (file) {
                try {
                    var extension = getFileExtension(file.name);
                    return this.options.imageExtensions.indexOf(extension) > -1;
                } catch (e) {
                    return false;
                }
            },

            initFabricUtil: function () {

                Fabric.Canvas = fabric.util.createClass(fabric.Canvas, {});

                Fabric.CropZone = fabric.util.createClass(fabric.Rect, {
                    _render: function (ctx) {
                        this.callSuper('_render', ctx);

                        var canvas = ctx.canvas;
                        var dashWidth = 7;

                        // Set original scale
                        var flipX = this.flipX ? -1 : 1;
                        var flipY = this.flipY ? -1 : 1;
                        var scaleX = flipX / this.scaleX;
                        var scaleY = flipY / this.scaleY;

                        ctx.scale(scaleX, scaleY);

                        // Overlay rendering
                        ctx.fillStyle = 'rgba(0, 0, 0, 0.5)';
                        this._renderOverlay(ctx);

                        // First lines rendering with black
                        ctx.strokeStyle = 'rgba(0, 0, 0, 0.2)';
                        this._renderBorders(ctx);

                        // Re render lines in white
                        ctx.lineDashOffset = dashWidth;
                        ctx.strokeStyle = 'rgba(255, 255, 255, 0.4)';
                        this._renderBorders(ctx);

                        // Reset scale
                        ctx.scale(1 / scaleX, 1 / scaleY);
                    },

                    _renderOverlay: function (ctx) {
                        var canvas = ctx.canvas;
                        var borderOffset = 0;

                        //
                        //    x0    x1        x2      x3
                        // y0 +------------------------+
                        //    |\\\\\\\\\\\\\\\\\\\\\\\\|
                        //    |\\\\\\\\\\\\\\\\\\\\\\\\|
                        // y1 +------+---------+-------+
                        //    |\\\\\\|         |\\\\\\\|
                        //    |\\\\\\|    0    |\\\\\\\|
                        //    |\\\\\\|         |\\\\\\\|
                        // y2 +------+---------+-------+
                        //    |\\\\\\\\\\\\\\\\\\\\\\\\|
                        //    |\\\\\\\\\\\\\\\\\\\\\\\\|
                        // y3 +------------------------+
                        //

                        var x0 = Math.ceil(-this.getWidth() / 2 - this.getLeft());
                        var x1 = Math.ceil(-this.getWidth() / 2);
                        var x2 = Math.ceil(this.getWidth() / 2);
                        var x3 = Math.ceil(this.getWidth() / 2 + (canvas.width - this.getWidth() - this.getLeft()));

                        var y0 = Math.ceil(-this.getHeight() / 2 - this.getTop());
                        var y1 = Math.ceil(-this.getHeight() / 2);
                        var y2 = Math.ceil(this.getHeight() / 2);
                        var y3 = Math.ceil(this.getHeight() / 2 + (canvas.height - this.getHeight() - this.getTop()));

                        // Upper rect
                        ctx.fillRect(x0, y0, x3 - x0, y1 - y0 + borderOffset);

                        // Left rect
                        ctx.fillRect(x0, y1, x1 - x0, y2 - y1 + borderOffset);

                        // Right rect
                        ctx.fillRect(x2, y1, x3 - x2, y2 - y1 + borderOffset);

                        // Down rect
                        ctx.fillRect(x0, y2, x3 - x0, y3 - y2);
                    },

                    _renderBorders: function (ctx) {
                        ctx.beginPath();
                        ctx.moveTo(-this.getWidth() / 2, -this.getHeight() / 2); // upper left
                        ctx.lineTo(this.getWidth() / 2, -this.getHeight() / 2); // upper right
                        ctx.lineTo(this.getWidth() / 2, this.getHeight() / 2); // down right
                        ctx.lineTo(-this.getWidth() / 2, this.getHeight() / 2); // down left
                        ctx.lineTo(-this.getWidth() / 2, -this.getHeight() / 2); // upper left
                        ctx.stroke();
                    },

                    _renderGrid: function (ctx) {
                        // Vertical lines
                        ctx.beginPath();
                        ctx.moveTo(-this.getWidth() / 2 + 1 / 3 * this.getWidth(), -this.getHeight() / 2);
                        ctx.lineTo(-this.getWidth() / 2 + 1 / 3 * this.getWidth(), this.getHeight() / 2);
                        ctx.stroke();
                        ctx.beginPath();
                        ctx.moveTo(-this.getWidth() / 2 + 2 / 3 * this.getWidth(), -this.getHeight() / 2);
                        ctx.lineTo(-this.getWidth() / 2 + 2 / 3 * this.getWidth(), this.getHeight() / 2);
                        ctx.stroke();
                        // Horizontal lines
                        ctx.beginPath();
                        ctx.moveTo(-this.getWidth() / 2, -this.getHeight() / 2 + 1 / 3 * this.getHeight());
                        ctx.lineTo(this.getWidth() / 2, -this.getHeight() / 2 + 1 / 3 * this.getHeight());
                        ctx.stroke();
                        ctx.beginPath();
                        ctx.moveTo(-this.getWidth() / 2, -this.getHeight() / 2 + 2 / 3 * this.getHeight());
                        ctx.lineTo(this.getWidth() / 2, -this.getHeight() / 2 + 2 / 3 * this.getHeight());
                        ctx.stroke();
                    }
                });
            },

            createFabricImage: function (img) {

                var width = img.width;
                var height = img.height;
                var scaleMin = 1;
                var scaleMax = 1;
                var scaleX = 1;
                var scaleY = 1;

                if (null !== this.options.maxWidth && this.options.maxWidth < width) {
                    scaleX = this.options.maxWidth / width;
                }
                if (null !== this.options.maxHeight && this.options.maxHeight < height) {
                    scaleY = this.options.maxHeight / height;
                }
                scaleMin = Math.min(scaleX, scaleY);

                scaleX = 1;
                scaleY = 1;
                if (null !== this.options.minWidth && this.options.minWidth > width) {
                    scaleX = this.options.minWidth / width;
                }
                if (null !== this.options.minHeight && this.options.minHeight > height) {
                    scaleY = this.options.minHeight / height;
                }
                scaleMax = Math.max(scaleX, scaleY);

                var scale = scaleMax * scaleMin; // one should be equals to 1

                width *= scale;
                height *= scale;

                var fabImage = new fabric.Image(img, {
                    // options to make the image static
                    selectable: false,
                    evented: false,
                    lockMovementX: true,
                    lockMovementY: true,
                    lockRotation: true,
                    lockScalingX: true,
                    lockScalingY: true,
                    lockUniScaling: true,
                    hasControls: false,
                    hasBorders: false,
                    originX: 'center',
                    originY: 'center',
                    left: width / 2,
                    top: height / 2
                });

                fabImage.setScaleX(scale);
                fabImage.setScaleY(scale);

                return fabImage;
            },

            // Avoid crop zone to go beyond the canvas edges
            onObjectMoving: function (event) {

                if (!this.hasFocus()) {
                    return;
                }

                var currentObject = event.target;
                if (currentObject !== this.cropZone)
                    return;

                var canvas = this.canvas;
                var img = canvas.item(0);

                var x = currentObject.getLeft(), y = currentObject.getTop();
                var w = currentObject.getWidth(), h = currentObject.getHeight();
                var maxX = img.getWidth() - w;
                var maxY = img.getHeight() - h;

                if (img.getWidth() > canvas.getWidth()) {
                    maxX = canvas.getWidth() - w;
                }

                if (img.getHeight() > canvas.getHeight()) {
                    maxY = canvas.getHeight() - h;
                }

                if (x < 0)
                    currentObject.set('left', 0);
                if (y < 0)
                    currentObject.set('top', 0);
                if (x > maxX)
                    currentObject.set('left', maxX);
                if (y > maxY)
                    currentObject.set('top', maxY);
            },

            // Prevent crop zone from going beyond the canvas edges (like mouseMove)
            onObjectScaling: function (event) {
                if (!this.hasFocus()) {
                    return;
                }

                var preventScaling = false;
                var currentObject = event.target;
                if (currentObject !== this.cropZone)
                    return;

                var canvas = this.canvas;
                var pointer = canvas.getPointer(event.e);
                var x = pointer.x;
                var y = pointer.y;

                var minX = currentObject.getLeft();
                var minY = currentObject.getTop();
                var maxX = currentObject.getLeft() + currentObject.getWidth();
                var maxY = currentObject.getTop() + currentObject.getHeight();

                var img = canvas.getObjects()[0];

                var maxWidth = img.getWidth();
                var maxHeight = img.getHeight();

                if (img.getWidth() > canvas.getWidth()) {
                    maxWidth = canvas.getWidth();
                }

                if (img.getHeight() > canvas.getHeight()) {
                    maxHeight = canvas.getHeight();
                }

                if (null !== this.options.crop.ratio) {
                    if (minX < 0 || maxX > maxWidth || minY < 0 || maxY > maxHeight) {
                        preventScaling = true;
                    }
                }

                if (minX < 0 || maxX > img.maxWidth || preventScaling) {
                    var lastScaleX = this.lastScaleX || 1;
                    currentObject.setScaleX(lastScaleX);
                }
                if (minX < 0) {
                    currentObject.setLeft(0);
                }

                if (minY < 0 || maxY > maxHeight || preventScaling) {
                    var lastScaleY = this.lastScaleY || 1;
                    currentObject.setScaleY(lastScaleY);
                }
                if (minY < 0) {
                    currentObject.setTop(0);
                }

                if (currentObject.getWidth() < this.options.minWidth) {
                    currentObject.scaleToWidth(this.options.minWidth);
                }
                if (currentObject.getHeight() < this.options.minHeight) {
                    currentObject.scaleToHeight(this.options.minHeight);
                }

                this.lastScaleX = currentObject.getScaleX();
                this.lastScaleY = currentObject.getScaleY();
            },

            selectZone: function (x, y, width, height, forceDimension) {
                if (!this.hasFocus())
                    this.requireFocus();

                if (!forceDimension) {
                    this._renderCropZone(x, y, x + width, y + height);
                } else {
                    this.cropZone.set({
                        'left': x,
                        'top': y,
                        'width': width,
                        'height': height
                    });
                }

                var canvas = this.canvas;
                canvas.bringToFront(this.cropZone);
                this.cropZone.setCoords();
                canvas.setActiveObject(this.cropZone);
                canvas.calcOffset();
            },

            toggleCrop: function () {
                if (!this.hasFocus())
                    this.requireFocus();
                else
                    this.releaseFocus();
            },

            cropCurrentZone: function (callback) {

                if (!this.hasFocus()) {
                    return;
                }

                // Avoid croping empty zone
                if (this.cropZone.width < 1 && this.cropZone.height < 1)
                    return;

                var _this = this;
                var canvas = this.canvas;

                // Hide crop rectangle to avoid snapshot it with the image
                this.cropZone.visible = false;

                // Snapshot the image delimited by the crop zone
                var image = new Image();
                image.onload = function () {
                    // Validate image
                    if (this.height < 1 || this.width < 1) {
                        return;
                    }

                    var imgInstance = new fabric.Image(this, {
                        // options to make the image static
                        selectable: false,
                        evented: false,
                        lockMovementX: true,
                        lockMovementY: true,
                        lockRotation: true,
                        lockScalingX: true,
                        lockScalingY: true,
                        lockUniScaling: true,
                        hasControls: false,
                        hasBorders: false
                    });

                    var width = this.width;
                    var height = this.height;

                    // Update canvas size
                    canvas.setWidth(width);
                    canvas.setHeight(height);

                    // Add image
                    _this.image.remove();
                    _this.image = imgInstance;
                    canvas.add(imgInstance);

                    callback();
                };

                image.src = canvas.toDataURL({
                    left: this.cropZone.getLeft(),
                    top: this.cropZone.getTop(),
                    width: this.cropZone.getWidth(),
                    height: this.cropZone.getHeight()
                });

                //new
                this.releaseFocus();

                return image;
            },

            zoom: function (scale) {
                var _this = this;
                var img = _this.canvas.getObjects()[0];

                this.canvas.forEachObject(function (obj) {

                    //zoom in
                    if (scale > 1) {
                        if (obj == _this.cropZone) {
                            if (obj.getHeight() + obj.getTop() > _this.canvas.getHeight() ||
                                obj.getWidth() + obj.getLeft() > _this.canvas.getWidth())
                                return;
                        }
                    }

                    obj.setScaleX(obj.getScaleX() * scale);
                    obj.setScaleY(obj.getScaleY() * scale);
                    obj.setTop(obj.getTop() * scale);
                    obj.setLeft(obj.getLeft() * scale);
                    obj.setCoords();
                });
                this.canvas.renderAll();
            },

            flipH: function () {
                var img = this.canvas.getObjects()[0];
                img.setFlipX(!img.getFlipX()).setCoords();
                this.canvas.renderAll();
            },

            flipV: function () {
                var img = this.canvas.getObjects()[0];
                img.setFlipY(!img.getFlipY()).setCoords();
                this.canvas.renderAll();
            },

            rotate: function (angle) {
                var img = this.canvas.getObjects()[0];
                img.setAngle(img.getAngle() + angle).setCoords();
                this.canvas.renderAll();
            },

            // Test wether crop zone is set
            hasFocus: function () {
                return this.cropZone !== undefined;
            },

            // Create the crop zone
            requireFocus: function () {
                this.cropZone = new Fabric.CropZone({
                    fill: 'transparent',
                    hasBorders: false,
                    originX: 'left',
                    originY: 'top',
                    //stroke: '#444',
                    //strokeDashArray: [5, 5],
                    //borderColor: '#444',
                    cornerColor: '#444',
                    cornerSize: 8,
                    transparentCorners: false,
                    lockRotation: true,
                    hasRotatingPoint: false,
                });

                if (null !== this.options.crop.ratio) {
                    this.cropZone.set('lockUniScaling', true);
                }

                this.canvas.add(this.cropZone);
                //this.IDPhotoMaker.canvas.defaultCursor = 'crosshair';

                //this.cropButton.active(true);
                //this.okButton.hide(false);
                //this.cancelButton.hide(false);
            },

            // Remove the crop zone
            releaseFocus: function () {
                if (undefined === this.cropZone)
                    return;

                this.cropZone.remove();
                this.cropZone = undefined;
                this.canvas.defaultCursor = 'default';
            },

            _renderCropZone: function (fromX, fromY, toX, toY) {
                var canvas = this.canvas;

                var isRight = (toX > fromX);
                var isDown = (toY > fromY);

                var minWidth = Math.min(+this.options.minWidth, canvas.getWidth());
                var minHeight = Math.min(+this.options.minHeight, canvas.getHeight());

                // Define corner coordinates
                var leftX = Math.min(fromX, toX);
                var rightX = Math.max(fromX, toX);
                var topY = Math.min(fromY, toY);
                var bottomY = Math.max(fromY, toY);

                // Replace current point into the canvas
                leftX = Math.max(0, leftX);
                rightX = Math.min(canvas.getWidth(), rightX);
                topY = Math.max(0, topY)
                bottomY = Math.min(canvas.getHeight(), bottomY);

                // Recalibrate coordinates according to given options
                if (rightX - leftX < minWidth) {
                    if (isRight)
                        rightX = leftX + minWidth;
                    else
                        leftX = rightX - minWidth;
                }
                if (bottomY - topY < minHeight) {
                    if (isDown)
                        bottomY = topY + minHeight;
                    else
                        topY = bottomY - minHeight;
                }

                // Truncate truncate according to canvas dimensions
                if (leftX < 0) {
                    // Translate to the left
                    rightX += Math.abs(leftX);
                    leftX = 0
                }
                if (rightX > canvas.getWidth()) {
                    // Translate to the right
                    leftX -= (rightX - canvas.getWidth());
                    rightX = canvas.getWidth();
                }
                if (topY < 0) {
                    // Translate to the bottom
                    bottomY += Math.abs(topY);
                    topY = 0
                }
                if (bottomY > canvas.getHeight()) {
                    // Translate to the right
                    topY -= (bottomY - canvas.getHeight());
                    bottomY = canvas.getHeight();
                }

                var width = this.options.crop.width;
                var height = this.options.crop.height;

                var currentRatio = height / width;

                if (this.options.crop.ratio && +this.options.crop.ratio !== currentRatio) {
                    var ratio = +this.options.crop.ratio;

                    if (currentRatio < ratio) {
                        width = height * ratio;
                    } else if (currentRatio > ratio) {
                        height = height / (ratio * height / width);
                    }

                    if (leftX < 0) {
                        leftX = 0;
                    }
                    if (topY < 0) {
                        topY = 0;
                    }
                    if (leftX + width > canvas.getWidth()) {
                        var newWidth = canvas.getWidth() - leftX;
                        height = newWidth * height / width;
                        width = newWidth;
                    }
                    if (topY + height > canvas.getHeight()) {
                        var newHeight = canvas.getHeight() - topY;
                        width = width * newHeight / height;
                        height = newHeight;
                    }
                }

                // Apply coordinates
                this.cropZone.left = leftX;
                this.cropZone.top = topY;
                this.cropZone.width = width;
                this.cropZone.height = height;

                this.canvas.bringToFront(this.cropZone);
            },

            selfDestroy: function () {
                var element = document.querySelector("#canvas_container");
                element.innerHTML = "";
            },

            snapshotImage: function () {
                return this.image.toDataURL({
                    format: this.options.outputImageFormat,
                    quality: this.options.outputImageQuality
                });
            },

            activeToolbar: function (isActive) {
                var buttons = $(".IDPhotoMaker-ui-editor-toolbar").find("input[type=button]");

                if (isActive)
                    buttons.removeAttr("disabled");
                else
                    buttons.attr("disabled", "disabled");
            },

            startWebcam: function () {
                Webcam.setBaseURL(getBaseUrl());
                Webcam.attach('.IDPhotoMaker-ui-camera-preview');
                Webcam.on("live", function () {
                    $("#camera_btn_start").attr("disabled", "disabled");
                    $("#camera_btn_stop").removeAttr("disabled");
                    $("#camera_btn_capture").show();
                });
            },

            stopWebcam: function () {
                try {
                    $("#camera_btn_start").removeAttr("disabled");
                    $("#camera_btn_stop").attr("disabled", "disabled");
                    $("#camera_btn_capture").hide();
                    Webcam.reset();
                } catch (e) { }
            },

            dispose: function () {
                this.stopWebcam();
            },

            validateImageSize: function (size) {
                size = (size / (1024 * 1024)).toFixed(2);
                return (size <= this.options.imageMaxSize);
            },

            isImageGrayScale: function (img)
            {
                var PHOTOS_MIN_GS = 80;
                var PHOTOS_SEUIL_RGB = 10;

                var canvas = document.createElement('canvas');
                var ctx = canvas.getContext("2d");

                canvas.width = img.width;
                canvas.height = img.height;

                ctx.drawImage(img, 0, 0);

                var data = ctx.getImageData(0, 0, img.width, img.height).data;
                var pxTotal = img.height * img.width;
                var pxCouleur = 0;
                var pxGS = 0;

                for (var i = 0, n = data.length; i < n; i += 4) {
                    var red = data[i];
                    var green = data[i + 1];
                    var blue = data[i + 2];
                    var alpha = data[i + 3];

                    if(alpha != 0 && Math.abs(red -green) >= PHOTOS_SEUIL_RGB || Math.abs(green -blue) >= PHOTOS_SEUIL_RGB || Math.abs(red -blue) >= PHOTOS_SEUIL_RGB) {
                        pxCouleur++;
                    }
                    else {
                        pxGS++;
                    }
                }

                var result = (pxGS / pxTotal) * 100;

                if (result >= PHOTOS_MIN_GS)
                    return true;
                else
                    return false;
            }
        }

    })();


    getBaseUrl = function () {
        var baseUrl = '';
        var scpts = document.getElementsByTagName('script');
        for (var idx = 0, len = scpts.length; idx < len; idx++) {
            var src = scpts[idx].getAttribute('src');
            if (src && src.match(/\/IDPhotoMaker\.js/)) {
                baseUrl = src.replace(/\/IDPhotoMaker\.js.*$/, '');
                idx = len;
            }
        }

        return baseUrl;
    };

    getTimestamp = function () {
        return new Date().getTime();
    };

    getFileExtension = function (fileName) {
        try {
            return fileName.substr(fileName.lastIndexOf('.') + 1).toLowerCase();
        } catch (e) {
            return false;
        }
    };

    loadScript = function(url, callback)
    {
        // Adding the script tag to the head as suggested before
        var head = document.getElementsByTagName('head')[0];
        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = url;

        // Then bind the event to the callback function.
        // There are several events for cross browser compatibility.
        script.onreadystatechange = callback;
        script.onload = callback;

        // Fire the loading
        head.appendChild(script);
    };

    if (!String.prototype.format) {
        String.prototype.format = function () {
            var args = arguments;
            return this.replace(/{(\d+)}/g, function (match, number) {
                return typeof args[number] != 'undefined'
                  ? args[number]
                  : match
                ;
            });
        };
    }

})(window, window.document);