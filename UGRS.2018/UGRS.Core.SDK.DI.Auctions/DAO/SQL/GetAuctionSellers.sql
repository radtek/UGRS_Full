  SELECT 'N' as Checked,T2.CardName,T2.CardCode,SUM(t0.U_Amount) as Amount,E_Mail FROM [@UG_SU_BAHS] T0
  INNER JOIN [@UG_SU_AUTN] T1 ON T1.U_Id = T0.U_AuctionId
  INNER JOIN OCRD T2 ON T2.CardName = T0.U_Seller

  WHERE U_Folio = '{Auction}'
  AND T0.U_Removed = 'N'
  AND T0.U_Unsold = 'N'

  group by t2.CardName,T2.CardCode,E_Mail
