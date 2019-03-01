


--Select USERID from OUSR where USER_CODE = '{UserCode}' and U_GLO_CostCenter = 'OG_COMPR'
-- Compras
Select USERID from OUSR T0 with (Nolock)
join OPRC T1 with (Nolock) on T0.U_GLO_CostCenter = T1.PrcCode
where T0.USER_CODE = '{UserCode}'
