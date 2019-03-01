--Acreedora
select * from OACT T0 with (Nolock)
inner join OACG T1 with (Nolock) on T0.Category = T1.AbsId
 where T1.Name = 'ACRED' and T0.Postable = 'Y'
 --and OverCode = '{Overcode}'
