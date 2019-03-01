SELECT 
	   T1.Name
      ,T0.[U_TypeA]
      ,T0.[U_TypeB]
      ,T0.[U_TypeC]
      ,T0.[U_TypeD]
      ,T0.[U_TypeE]
      ,T0.[U_TypeF]
      ,T0.[U_Commission]
	  ,T0.U_EquipType
  FROM [@UG_TR_VETY] T0
  inner join [@UG_GLO_EQUIPO] T1 on T0.U_EquipType = t1.Code
