
--select * from OWHT where round(Rate/PrctBsAmnt, 1, 1) = '{Rate}'
select * from CRD4 T0 with (Nolock)
inner join OWHT T1 with (Nolock) on T1.WTCode = T0.WTCode
where T0.CardCode = '{CardCode}' and round(Rate/PrctBsAmnt, 1, 1) = '{Rate}'