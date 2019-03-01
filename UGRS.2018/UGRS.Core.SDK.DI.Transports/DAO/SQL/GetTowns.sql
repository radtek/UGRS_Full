SELECT
      [Name]
      ,[U_State]
      ,[U_Commite]
  FROM [@UG_GLO_TOWN]
  WHERE 
 Name = COALESCE(NULLIF('{Town}',''),Name) 
 AND
(Name like '%{Search}%' or U_State like '%{Search}%' or U_Commite like '%{Search}%')