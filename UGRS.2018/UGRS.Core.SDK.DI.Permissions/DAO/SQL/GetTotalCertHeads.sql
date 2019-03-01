select sum(Quantity) from RDR1  T0
inner join OITM T1 On T0.ItemCode = T1.ItemCode
where T0.DocEntry = '{DocEntry}' and T1.QryGroup13 = 'Y'