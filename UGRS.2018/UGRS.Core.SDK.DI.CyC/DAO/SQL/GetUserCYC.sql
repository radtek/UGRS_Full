

select T0.USERID, T0.USER_CODE
from OUSR T0
inner join OHEM T1 on T0.USERID = T1.userId
inner join HEM6 T2 on T2.empID = T1.empID
inner join OHTY T3 on T3.typeID = T2.roleID
where T3.descriptio Like 'CYCSU' AND USER_CODE = '{UserCode}'
