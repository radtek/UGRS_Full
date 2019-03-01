
select T1.empID from OHTY T0 with (Nolock) 
join HEM6 T1 with (Nolock) on T0.typeID = T1.roleID
join OHEM T2 with (Nolock) on T1.empID = T2.empID
where T0.descriptio Like 'ENCFF' and T2.dept = '{dept}'