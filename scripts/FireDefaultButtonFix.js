function WebForm_FireDefaultButton(event, target) 
{
    //event.srcElement doesn't work in FF so we check whether
    //it or event.target exists, using whichever is returned
    var element = event.target || event.srcElement;
    
    if (event.keyCode == 13 &&
        !(element &&
        element.tagName.toLowerCase() == "textarea")) 
        {
        var defaultButton;
        if (__nonMSDOMBrowser) 
        {
            defaultButton = document.getElementById(target);
        } 
        else 
        {
            defaultButton = document.all[target];
        }
        if (defaultButton && typeof defaultButton.click != "undefined") 
        {
            defaultButton.click();
            event.cancelBubble = true;
            if (event.stopPropagation) 
            {
                event.stopPropagation();
            }
            return false;
        }
    }
    return true;
}
