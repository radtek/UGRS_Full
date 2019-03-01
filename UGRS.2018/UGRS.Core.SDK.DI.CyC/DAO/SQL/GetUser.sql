
select t0.USERID, t0.USER_CODE, max(Department) Department, 
  max(t1.Name) Name, max(t0.U_GLO_CostCenter) U_GLO_CostCenter,
  max(case when T4.typeID is not null then 'Y' else 'N' end) CYC
from OUSR T0
inner join OUDP T1 on T0.Department = T1.Code 
inner join OHEM T2 on T0.USERID = T2.userId
left join HEM6 T3 on T3.empID = T2.empID
left join OHTY T4 on T4.typeID = T3.roleID and cast(T4.descriptio as nvarchar(15)) = 'CYCSU'
where t0.USER_CODE = '{UserCode}'
group by t0.USERID, t0.USER_CODE 