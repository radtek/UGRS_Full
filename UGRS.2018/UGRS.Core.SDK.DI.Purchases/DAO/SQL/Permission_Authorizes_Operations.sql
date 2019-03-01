--Autoriza operaciones
select T0.empID from OHEM  T0 with (Nolock)
join OUSR T1 with (Nolock) on T0.userId = T1.USERID
join HEM6 T2 with (Nolock) on T0.empID = T2.empID
join OHTY T3 with (Nolock) on T3.typeID = T2.roleID
join OPRC T4 with (Nolock) on T1.U_GLO_CostCenter = T4.PrcCode
where  T1.USER_CODE = '{UserCode}'  and T3.descriptio like 'AUTCO'
