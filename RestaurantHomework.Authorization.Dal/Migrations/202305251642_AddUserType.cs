using FluentMigrator;

namespace RestaurantHomework.Authorization.Dal.Migrations;

[Migration(202305251642, TransactionBehavior.None)]
public class AddUserType : Migration {
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'users_v1') THEN
            CREATE TYPE users_v1 as
            (
                  id           int
                , username     varchar(50)
                , email        varchar(100)
                , password_hash varchar(255)
                , role         varchar(10)
                , created_at   timestamp
                , updated_at   timestamp
            );
        END IF;
    END
$$;";

        Execute.Sql(sql);
    }

    public override void Down()
    {
        const string sql = @"
DO $$
    BEGIN
        DROP TYPE IF EXISTS users_v1;
    END
$$;";

        Execute.Sql(sql);
    }
}