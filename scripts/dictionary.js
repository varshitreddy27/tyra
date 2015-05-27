//===========================================================================
// Provides a Dictionary object for client-side java scripts
//===========================================================================

function Lookup(key) 
{
  return(this[key]);
}


function Delete() 
{
  for (c=0; c < Delete.arguments.length; c++) 
  {
    this[Delete.arguments[c]] = null;
  }
  // Adjust the keys (not terribly efficient)
  var keys = new Array()
  for (var i=0; i<this.Keys.length; i++)
  {
    if(this[this.Keys[i]] != null)
      keys[keys.length] = this.Keys[i];
  }
  this.Keys = keys;
}

function Add() 
{
  for (c=0; c < Add.arguments.length; c+=2) 
  {
    // Add the property
    this[Add.arguments[c]] = Add.arguments[c+1];
    // And add it to the keys array
    this.Keys[this.Keys.length] = Add.arguments[c];
  }
}

function Dictionary() 
{
  this.Add = Add;
  this.Lookup = Lookup;
  this.Delete = Delete;
  this.Keys = new Array();
}

