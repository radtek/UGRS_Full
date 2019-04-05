select t1.Warehouse from ousr T0 with (Nolock)
inner join OUDG T1 with (Nolock) on t0.DfltsGroup = t1.Code
inner join OWHS T2 with (Nolock) on t2.whscode =t1.Warehouse

where t0.USERID='{UsrId}'