-- Task 2, query 1
SELECT 
    ClientName, 
	(
      SELECT count(*) 
      FROM ClientContacts 
      WHERE ClientContacts.ClientId = Clients.Id
    ) AS cont_count
FROM
Clients;

-- Task 2, query 2
SELECT 
  Clients.Id,
  Clients.ClientName,
  cont.cont_count
FROM
(
  SELECT 
    ClientId, COUNT(ClientId) AS cont_count
  FROM
    ClientContacts 
  GROUP BY
    ClientId
  HAVING COUNT(ClientId) > 2
  ORDER BY
    ClientId
) AS cont
INNER JOIN Clients ON cont.ClientId = Clients.Id;
	
