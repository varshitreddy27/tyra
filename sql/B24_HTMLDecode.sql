Create FUNCTION B24_HTMLDecode (@vcWhat varchar(8000))
RETURNS varchar(8000) AS  
/*===========================================================================================================================
This function converts ASCII values in a text to text
example: 
"LDC, T&#252;rk  - Trial" will be converted as "LDC, Türk  - Trial"
===============================================================================================================================*/
BEGIN 
DECLARE @vcResult varchar(8000)
DECLARE @vcCrLf varchar(2)
DECLARE @siPos smallint,@vcEncoded varchar(7),@siChar smallint

set @vcCrLF=char(13) + char(10)

select @vcResult=@vcWhat
select @siPos=PatIndex('%&#___;%',@vcResult)
WHILE @siPos>0
  BEGIN
      select @vcEncoded=substring(@vcResult,@siPos,6)
      select @siChar=cast(substring(@vcEncoded,3,3) as smallint)
      select @vcResult=replace(@vcResult,@vcEncoded,char(@siChar))
      select @siPos=PatIndex('%&#___;%',@vcResult)
  END

select @siPos=PatIndex('%&#____;%',@vcResult)
WHILE @siPos>0
  BEGIN
      select @vcEncoded=substring(@vcResult,@siPos,7)
      select @siChar=cast(substring(@vcEncoded,3,4) as smallint)
      select @vcResult=replace(@vcResult,@vcEncoded,char(@siChar))
      select @siPos=PatIndex('%&#____;%',@vcResult)
  END
return @vcResult
END


