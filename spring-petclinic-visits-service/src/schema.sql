CREATE DATABASE petclinic;

USE petclinic;

CREATE TABLE visits (
  id          INTEGER IDENTITY PRIMARY KEY,
  pet_id      INTEGER NOT NULL,
  visit_date  DATE,
  description VARCHAR(8000)
);

CREATE INDEX visits_pet_id ON visits (pet_id);
