select OSTC.Rate from OTCX with (Nolock)
inner join OSTC with (Nolock) on OTCX.LnTaxCode = OSTC.code
where Cond1 = '9' and  StrVal1 = '{ItemCode}'