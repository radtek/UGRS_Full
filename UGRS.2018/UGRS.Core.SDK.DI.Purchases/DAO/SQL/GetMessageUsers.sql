select T1.USER_CODE  from OHEM  T0 with (Nolock)
join OUSR T1 with (Nolock) on T0.userId = T1.USERID
join HEM6 T2 with (Nolock) on T0.empID = T2.empID
join OHTY T3 with (Nolock) on T3.typeID = T2.roleID
where  T1.U_GLO_CostCenter = '{CostCenter}' and T3.descriptio like 'AUTCO'
