SELECT
      [U_TR_TOWN] as Name
      ,[U_TR_State] as U_State
  FROM [@UG_TR_TOWN] with (Nolock)
  WHERE 
 [U_TR_TOWN] = COALESCE(NULLIF('{Town}',''),[U_TR_TOWN]) 
 AND
([U_TR_TOWN] like '%%' or [U_TR_State] like '%{Search}%')

--SELECT
--      [Name]
--      ,[U_State]
--      ,[U_Commite]
--  FROM [@UG_GLO_TOWN] with (Nolock)
--  WHERE 
-- Name = COALESCE(NULLIF('{Town}',''),Name) 
-- AND
--(Name like '%{Search}%' or U_State like '%{Search}%' or U_Commite like '%{Search}%')