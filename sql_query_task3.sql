-- Task 3
SELECT 
  Id, sd, ed
FROM (
	SELECT DISTINCT
	  Id, 
	  Dt AS sd,  
	  LEAD(Dt) OVER (
		PARTITION BY Id 
		ORDER BY Id, Dt) AS ed
	FROM
	  Dates
	ORDER BY
	  Id, Dt
) AS d
WHERE
  ed IS NOT NULL; 