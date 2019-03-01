select T1.Rate from OTCX T0
join OSTC T1 on T0.LnTaxCode = T1.Code
where BusArea = '0' and StrVal1 =  '{ItemCode}' 