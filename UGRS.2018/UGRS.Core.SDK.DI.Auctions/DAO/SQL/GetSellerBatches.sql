  SELECT DISTINCT 
  t2.U_Date,
  T2.U_Folio,
  T0.U_Number,
  T0.U_Quantity,
  T0.U_ItemType,
  T0.U_AverageWeight,
  T0.U_Weight,
  T0.U_Price,
  T0.U_Amount,
  T0.U_Unsold, 
  T0.U_UnsoldMotive,
  T0.U_Gender, 
  t0.U_Buyer +', RFC:'+t1.LicTradNum+', DIR:'+ t1.Address +' '+ t1.StreetNo+', COLONIA:'+t1.Block +', CD:'+t1.ZipCode as U_Buyer,  
  t0.U_Reprogrammed,
  CASE WHEN t0.U_Unsold = 'N' and t0.U_Reprogrammed = 'Y' THEN 'Reprogramado' 
  WHEN t0.U_Unsold = 'Y' THEN 'No vendidos'
  WHEN t0.U_Unsold = 'N' and t0.U_Reprogrammed = 'N' THEN 'Vendidos' END AS Stat ,
    CASE WHEN t0.U_Unsold = 'N' and t0.U_Reprogrammed = 'Y' THEN 3
  WHEN t0.U_Unsold = 'Y' THEN 2
  WHEN t0.U_Unsold = 'N' and t0.U_Reprogrammed = 'N' THEN 1 END AS Orden


  FROM [@UG_SU_BAHS] T0
  INNER JOIN OCRD T1 ON T1.CardName = T0.U_Seller
  left JOIN [@UG_SU_AUTN] T2 ON t2.U_Id = t0.U_AuctionId 

  where  (t2.U_Folio = '{Auction}' AND t1.CardCode = '{Seller}' and (t0.U_Unsold != 'Y' or t0.U_Reprogrammed != 'Y'))