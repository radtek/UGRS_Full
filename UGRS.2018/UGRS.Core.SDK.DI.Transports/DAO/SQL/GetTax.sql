select OSTC.Rate from OTCX 
inner join OSTC on OTCX.LnTaxCode = OSTC.code
where Cond1 = '9' and  StrVal1 = '{ItemCode}'