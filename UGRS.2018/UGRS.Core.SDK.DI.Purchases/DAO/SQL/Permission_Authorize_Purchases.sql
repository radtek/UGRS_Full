--Autoriza compras
select T0.empID from OHEM  T0 with (Nolock)
join OUSR T1 with (Nolock) on T0.userId = T1.USERID
join OPRC T2 with (Nolock) on T1.U_GLO_CostCenter = T2.PrcCode
where  T1.USER_CODE = '{UserCode}' and  T1.U_GLO_CostCenter = '{CostCenter}'-- and T3.descriptio like 'AUTCO'
