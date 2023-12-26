CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "AccountRelations" (
    "Account" text NOT NULL,
    "ConnectedAccount" text NOT NULL,
    "RequestSent" bool NOT NULL,
    "Approved" bool NOT NULL,
    CONSTRAINT "PK_AccountRelations" PRIMARY KEY ("Account")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20231219114420_relations', '7.0.12');

COMMIT;

