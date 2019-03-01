--Codigo de clasificacion
select ItemCode, ItemName from OITM T0 with (Nolock)
join ONCM T1 with (Nolock) on T1.AbsEntry = T0.NCMCode
where PrchseItem = 'Y' and T1.NcmCode = '{ClasificationCode}' 