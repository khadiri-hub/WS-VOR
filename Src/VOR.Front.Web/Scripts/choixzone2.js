// FirstLoad Handle Highlight
HighLightHandle();

// Reload highlightMap after asynchrone postback !!!
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(highlightMap);

function highlightMap() {

    (function (G) { var B, J, C, K, N, M, I, E, H, A, L; B = document.namespaces; J = !!document.createElement("canvas").getContext; if (!(J || B)) { G.fn.maphilight = function () { return this }; return } if (J) { E = function (O) { return Math.max(0, Math.min(parseInt(O, 16), 255)) }; H = function (O, P) { return "rgba(" + E(O.substr(0, 2)) + "," + E(O.substr(2, 2)) + "," + E(O.substr(4, 2)) + "," + P + ")" }; C = function (O) { var P = G('<canvas style="width:' + O.width + "px;height:" + O.height + 'px;"></canvas>').get(0); P.getContext("2d").clearRect(0, 0, P.width, P.height); return P }; var F = function (Q, O, R, P, S) { P = P || 0; S = S || 0; Q.beginPath(); if (O == "rect") { Q.rect(R[0] + P, R[1] + S, R[2] - R[0], R[3] - R[1]) } else { if (O == "poly") { Q.moveTo(R[0] + P, R[1] + S); for (i = 2; i < R.length; i += 2) { Q.lineTo(R[i] + P, R[i + 1] + S) } } else { if (O == "circ") { Q.arc(R[0] + P, R[1] + S, R[2], 0, Math.PI * 2, false) } } } Q.closePath() }; K = function (Q, T, U, X, O) { var S, P = Q.getContext("2d"); if (X.shadow) { P.save(); if (X.shadowPosition == "inside") { F(P, T, U); P.clip() } var R = Q.width * 100; var W = Q.height * 100; F(P, T, U, R, W); P.shadowOffsetX = X.shadowX - R; P.shadowOffsetY = X.shadowY - W; P.shadowBlur = X.shadowRadius; P.shadowColor = H(X.shadowColor, X.shadowOpacity); var V = X.shadowFrom; if (!V) { if (X.shadowPosition == "outside") { V = "fill" } else { V = "stroke" } } if (V == "stroke") { P.strokeStyle = "rgba(0,0,0,1)"; P.stroke() } else { if (V == "fill") { P.fillStyle = "rgba(0,0,0,1)"; P.fill() } } P.restore(); if (X.shadowPosition == "outside") { P.save(); F(P, T, U); P.globalCompositeOperation = "destination-out"; P.fillStyle = "rgba(0,0,0,1);"; P.fill(); P.restore() } } P.save(); F(P, T, U); if (X.fill) { P.fillStyle = H(X.fillColor, X.fillOpacity); P.fill() } if (X.stroke) { P.strokeStyle = H(X.strokeColor, X.strokeOpacity); P.lineWidth = X.strokeWidth; P.stroke() } P.restore(); if (X.fade) { G(Q).css("opacity", 0).animate({ opacity: 1 }, 100) } }; N = function (O) { O.getContext("2d").clearRect(0, 0, O.width, O.height) } } else { C = function (O) { return G('<var style="zoom:1;overflow:hidden;display:block;width:' + O.width + "px;height:" + O.height + 'px;"></var>').get(0) }; K = function (P, S, T, W, O) { var U, V, Q, R; U = '<v:fill color="#' + W.fillColor + '" opacity="' + (W.fill ? W.fillOpacity : 0) + '" />'; V = (W.stroke ? 'strokeweight="' + W.strokeWidth + '" stroked="t" strokecolor="#' + W.strokeColor + '"' : 'stroked="f"'); Q = '<v:stroke opacity="' + W.strokeOpacity + '"/>'; if (S == "rect") { R = G('<v:rect name="' + O + '" filled="t" ' + V + ' style="zoom:1;margin:0;padding:0;display:block;position:absolute;left:' + T[0] + "px;top:" + T[1] + "px;width:" + (T[2] - T[0]) + "px;height:" + (T[3] - T[1]) + 'px;"></v:rect>') } else { if (S == "poly") { R = G('<v:shape name="' + O + '" filled="t" ' + V + ' coordorigin="0,0" coordsize="' + P.width + "," + P.height + '" path="m ' + T[0] + "," + T[1] + " l " + T.join(",") + ' x e" style="zoom:1;margin:0;padding:0;display:block;position:absolute;top:0px;left:0px;width:' + P.width + "px;height:" + P.height + 'px;"></v:shape>') } else { if (S == "circ") { R = G('<v:oval name="' + O + '" filled="t" ' + V + ' style="zoom:1;margin:0;padding:0;display:block;position:absolute;left:' + (T[0] - T[2]) + "px;top:" + (T[1] - T[2]) + "px;width:" + (T[2] * 2) + "px;height:" + (T[2] * 2) + 'px;"></v:oval>') } } } R.get(0).innerHTML = U + Q; G(P).append(R) }; N = function (O) { G(O).find("[name=highlighted]").remove() } } M = function (P) { var O, Q = P.getAttribute("coords").split(","); for (O = 0; O < Q.length; O++) { Q[O] = parseFloat(Q[O]) } return [P.getAttribute("shape").toLowerCase().substr(0, 4), Q] }; L = function (Q, P) { var O = G(Q); return G.extend({}, P, G.metadata ? O.metadata() : false, O.data("maphilight")) }; A = function (O) { if (!O.complete) { return false } if (typeof O.naturalWidth != "undefined" && O.naturalWidth == 0) { return false } return true }; I = { position: "absolute", left: 0, top: 0, padding: 0, border: 0 }; var D = false; G.fn.maphilight = function (Q) { Q = G.extend({}, G.fn.maphilight.defaults, Q); if (!J && G.browser.msie && !D) { document.namespaces.add("v", "urn:schemas-microsoft-com:vml"); var P = document.createStyleSheet(); var O = ["shape", "rect", "oval", "circ", "fill", "stroke", "imagedata", "group", "textbox"]; G.each(O, function () { P.addRule("v\\:" + this, "behavior: url(#default#VML); antialias:true") }); D = true } return this.each(function () { var W, T, a, S, V, X, Z, U, Y; W = G(this); if (!A(this)) { return window.setTimeout(function () { W.maphilight(Q) }, 200) } a = G.extend({}, Q, G.metadata ? W.metadata() : false, W.data("maphilight")); Y = W.get(0).getAttribute("usemap"); S = G('map[name="' + Y.substr(1) + '"]'); if (!(W.is("img") && Y && S.size() > 0)) { return } if (W.hasClass("maphilighted")) { var R = W.parent(); W.insertBefore(R); R.remove(); G(S).unbind(".maphilight").find("area[coords]").unbind(".maphilight") } T = G("<div></div>").css({ display: "block", background: 'url("' + this.src + '")', position: "relative", padding: 0, width: this.width, height: this.height }); if (a.wrapClass) { if (a.wrapClass === true) { T.addClass(G(this).attr("class")) } else { T.addClass(a.wrapClass) } } W.before(T).css("opacity", 0).css(I).remove(); if (G.browser.msie) { W.css("filter", "Alpha(opacity=0)") } T.append(W); V = C(this); G(V).css(I); V.height = this.height; V.width = this.width; Z = function (f) { var c, d; d = L(this, a); if (!d.neverOn && !d.alwaysOn) { c = M(this); K(V, c[0], c[1], d, "highlighted"); if (d.groupBy) { var b; if (/^[a-zA-Z][-a-zA-Z]+$/.test(d.groupBy)) { b = S.find("area[" + d.groupBy + '="' + G(this).attr(d.groupBy) + '"]') } else { b = S.find(d.groupBy) } var g = this; b.each(function () { if (this != g) { var h = L(this, a); if (!h.neverOn && !h.alwaysOn) { var e = M(this); K(V, e[0], e[1], h, "highlighted") } } }) } if (!J) { G(V).append("<v:rect></v:rect>") } } }; G(S).bind("alwaysOn.maphilight", function () { if (X) { N(X) } if (!J) { G(V).empty() } G(S).find("area[coords]").each(function () { var b, c; c = L(this, a); if (c.alwaysOn) { if (!X && J) { X = C(W.get()); G(X).css(I); X.width = W.width(); X.height = W.height(); W.before(X) } c.fade = c.alwaysOnFade; b = M(this); if (J) { K(X, b[0], b[1], c, "") } else { K(V, b[0], b[1], c, "") } } }) }); G(S).trigger("alwaysOn.maphilight").find("area[coords]").bind("mouseover.maphilight", Z).bind("mouseout.maphilight", function (b) { N(V) }); W.before(V); W.addClass("maphilighted") }) }; G.fn.maphilight.defaults = { fill: true, fillColor: "000000", fillOpacity: 0.2, stroke: true, strokeColor: "ff0000", strokeOpacity: 1, strokeWidth: 1, fade: true, alwaysOn: false, neverOn: false, groupBy: false, wrapClass: true, shadow: false, shadowX: 0, shadowY: 0, shadowRadius: 6, shadowColor: "000000", shadowOpacity: 0.8, shadowPosition: "outside", shadowFrom: false} })(jQuery);

    HighLightHandle();

}

function HighLightHandle() {

    // Style CSS :hover de l'area map.
    $(function () {

        $.fn.maphilight.defaults = {
            fill: true,
            fillColor: '1EB90D',
            fillOpacity: 0.2,
            stroke: true,
            strokeColor: '333333',
            strokeOpacity: 1,
            strokeWidth: 1,
            fade: true,
            alwaysOn: false,
            neverOn: false,
            groupBy: false,
            wrapClass: true,
            shadow: false,
            shadowX: 0,
            shadowY: 0,
            shadowRadius: 6,
            shadowColor: '000000',
            shadowOpacity: 0.8,
            shadowPosition: 'outside',
            shadowFrom: false
        }

        $('.map').maphilight();
        });
}