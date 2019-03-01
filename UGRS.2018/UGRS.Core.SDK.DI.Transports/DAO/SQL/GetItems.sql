SELECT DISTINCT t0.ItemCode, t0.ItemName, t0.U_GLO_RetTax from OITM T0
LEFT JOIN OITW t1 on t0.ItemCode = t1.ItemCode
INNER JOIN [@UG_TR_ITEMRELATION] T2 on t0.ItemCode = t2.Name
INNER JOIN OCRD t3 on t3.CmpPrivate = t2.U_PartnerType
where t1.WhsCode = '{WHS}' and t1.Locked = 'N' AND t0.SellItem ='Y' AND t0.ItemType = 'I'
AND T3.CardCode = COALESCE(NULLIF('{CardCode}',''),T3.CardCode) 
AND T0.ItemCode = COALESCE(NULLIF('{ItemCode}',''),T0.ItemCode) 
AND ( t0.ItemCode like '%{Search}%' or t0.ItemName like '%{Search}%')