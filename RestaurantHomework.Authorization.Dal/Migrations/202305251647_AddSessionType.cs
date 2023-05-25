using FluentMigrator;

namespace RestaurantHomework.Authorization.Dal.Migrations;

[Migration(202305251647, TransactionBehavior.None)]
public class AddSessionType : Migration {
    public override void Up()
    {
        const string sql = @"
DO $$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'sessions_v1') THEN
            CREATE TYPE sessions_v1 as
            (
                  id            int
                , user_id       int
                , session_token varchar(255)
                , expires_at    timestamp
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
        DROP TYPE IF EXISTS sessions_v1;
    END
$$;";

        Execute.Sql(sql);
    }
}