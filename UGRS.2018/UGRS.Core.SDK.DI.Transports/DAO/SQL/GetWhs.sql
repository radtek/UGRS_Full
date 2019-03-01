select t1.Warehouse from ousr T0
inner join OUDG T1 on t0.DfltsGroup = t1.Code
inner join OWHS T2 on t2.whscode =t1.Warehouse

where t0.USERID='{UsrId}'