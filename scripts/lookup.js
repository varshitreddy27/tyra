// Functions used on the lookup page

var userDict = new Dictionary();
var subDict = new Dictionary();



//==========================================================
// Initilize the given dictionary object with the commma-
// separated list of id's in the string
// 
// dict:    the dictionary to initialize
// str:     comma-sep list of id's to add to dict
//==========================================================
function InitDict(dict, str)
{
  var a = str.split(",");
  for(var i=0; i<a.length; i++)
  {
    dict.Add(a[i], 1);
  }
}

//==========================================================
// Add or remove a user from the cart
// 
// elem:    -- the checkbox input containing the user's id
//==========================================================
function CartUser(elem)
{
  if(elem)
  { 
    // Make sure the dictionary is initialized properly
    if(userDict.Keys.length == 0 && document.forms["cartform"].userCart.value.length > 0)
      InitDict(userDict, document.forms["cartform"].userCart.value);
  
    // Add / remove the user
    if(elem.checked)
      userDict.Add(elem.value, 1);
    else
      userDict.Delete(elem.value)
    
    // Update the cart    
    var keys = userDict.Keys
    document.forms["cartform"].userCart.value = ""
    for(var i=0; i<keys.length; i++)
    {
      if(document.forms["cartform"].userCart.value.length > 0)
        document.forms["cartform"].userCart.value += ","+keys[i];
      else
        document.forms["cartform"].userCart.value += keys[i];
    }

    //alert("userCart: "+ document.forms["cartform"].userCart.value;

  }
}

//==========================================================
// Add or remove a sub from the cart
// 
// elem:    -- the checkbox input containing the sub's id
//==========================================================
function CartSub(elem)
{
  if(elem)
  {
    // Make sure the dictionary is initialized properly
    if(subDict.Keys.length == 0 && document.forms["cartform"].subCart.value.length > 0)
      InitDict(subDict, document.forms["cartform"].subCart.value);
  
    // Add / remove the sub
    if(elem.checked)
      subDict.Add(elem.value, 1);
    else
      subDict.Delete(elem.value)
    
    // Update the cart    
    var keys = subDict.Keys
    document.forms["cartform"].subCart.value = ""
    for(var i=0; i<keys.length; i++)
    {
      if(document.forms["cartform"].subCart.value.length > 0)
        document.forms["cartform"].subCart.value += ","+keys[i];
      else
        document.forms["cartform"].subCart.value += keys[i];
    }
    
    
    //alert("subCart: "+ document.forms["cartform"].subCart.value;
  }
}
