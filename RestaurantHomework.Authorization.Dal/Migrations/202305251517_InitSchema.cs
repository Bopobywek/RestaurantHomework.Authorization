using FluentMigrator;

namespace RestaurantHomework.Authorization.Dal.Migrations;

[Migration(202305251517, TransactionBehavior.None)]
public class InitSchema : Migration {
    public override void Up()
    {
        Execute.Sql(@"CREATE TABLE users (
          id SERIAL PRIMARY KEY,
          username VARCHAR(50) UNIQUE NOT NULL,
          email VARCHAR(100) UNIQUE NOT NULL,
          password_hash VARCHAR(255) NOT NULL,
          role VARCHAR(10) NOT NULL CHECK (role IN ('customer', 'chef', 'manager')),
          created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
          updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
 
CREATE TABLE sessions (
    id SERIAL PRIMARY KEY,
    user_id INT NOT NULL,
    session_token VARCHAR(1024) NOT NULL,
    expires_at TIMESTAMP NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users(id)
);
 
CREATE OR REPLACE FUNCTION update_update_at_column()
RETURNS TRIGGER AS '
BEGIN
   NEW.updated_at = NOW(); 
   RETURN NEW;
END;' 
LANGUAGE 'plpgsql';
 
CREATE TRIGGER update_users_update_at BEFORE UPDATE
ON users FOR EACH ROW EXECUTE PROCEDURE 
update_update_at_column();"
        );
    }

    public override void Down()
    {
        Execute.Sql(@"DROP TABLE user");
        Execute.Sql(@"DROP TABLE session");
    }
}