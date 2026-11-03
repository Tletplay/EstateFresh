UPDATE Orders 
SET DriverID = 13
WHERE DriverID IS NULL AND Status = 'Awaiting Pickup';