// This function is used for autocompleteextender to keep  dropdownlist popup opened while the user has not chosen an option
// Add a BehaviourID to the autocompleteextender component
// Add a  $find($BehaviourID).hidePopup(); with $BehaviourID defined into autocompleteextender : ex BehaviourID="ACSociete" , find("ACSociete").hidePopup();

//These two functions is copied from the original design code of the AutoCompleteBehavior.

//We modify them to insert the selected value and keep the CompletionList shown.

//Note that we need to place the script tag under the ScriptManager and the AutoCompleteExtender

// to ensure we can use the AjaxControlToolkit namespace.

if (typeof (Sys) !== 'undefined' && typeof (Sys.Extended) !== 'undefined' && typeof (Sys.Extended.UI) !== 'undefined' && typeof (Sys.Extended.UI.AutoCompleteBehavior) !== 'undefined') {
    Sys.Extended.UI.AutoCompleteBehavior.prototype._setText = function (item) {
        //Rewrite the _setText function to insert the newText into the ListBox.
        var text = (item && item.firstChild) ? item.firstChild.nodeValue : null;
        this._timer.set_enabled(false);

        var element = this.get_element();
        var control = element.control;

        var newText = this._showOnlyCurrentWordInCompletionListItem ? this._getTextWithInsertedWord(text) : text;
        if (control && control.set_text) {
            control.set_text(newText);
        } else {
            element.value = newText;
        }

        $common.tryFireEvent(element, "change");

        this.raiseItemSelected(new Sys.Extended.UI.AutoCompleteItemEventArgs(item, text, item ? item._value : null));

        this._currentPrefix = this._currentCompletionWord();
        this._hideCompletionList();
    };
    Sys.Extended.UI.AutoCompleteBehavior.prototype._hideCompletionList = function () {
        //Rewrite the _hideCompletionList function to keep the list shown all the time

        var eventArgs = new Sys.CancelEventArgs();
        this.raiseHiding(eventArgs);
        if (eventArgs.get_cancel()) {
            return;
        }
        //The hidePopup function is to close the CompletionList, so we need to
        // comment this line to ensure the CompletionList is visible.
        //this.hidePopup();
    };
}