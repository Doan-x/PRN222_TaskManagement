using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN222_TaskManagement.Migrations
{
    /// <inheritdoc />
    public partial class v0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Labels",
                columns: table => new
                {
                    label_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Labels__E44FFA5803D957CF", x => x.label_id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "user"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__B9BE370F28BE3159", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "CalendarSyncs",
                columns: table => new
                {
                    sync_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    service_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    access_token = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    refresh_token = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    last_synced = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Calendar__54E41ED02F3C5CB1", x => x.sync_id);
                    table.ForeignKey(
                        name: "FK__CalendarS__user___6D0D32F4",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__D54EE9B4E284E110", x => x.category_id);
                    table.ForeignKey(
                        name: "FK__Categorie__user___5165187F",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    task_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    due_date = table.Column<DateOnly>(type: "date", nullable: true),
                    completed_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(NULL)"),
                    priority = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "medium"),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "pending"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tasks__0492148DEBC5EF70", x => x.task_id);
                    table.ForeignKey(
                        name: "FK__Tasks__user_id__7C4F7684",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    event_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    start_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    priority = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "medium"),
                    color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Events__2370F7278127D8A1", x => x.event_id);
                    table.ForeignKey(
                        name: "FK__Events__category__59063A47",
                        column: x => x.category_id,
                        principalTable: "Categories",
                        principalColumn: "category_id");
                    table.ForeignKey(
                        name: "FK__Events__user_id__5812160E",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "TaskLabels",
                columns: table => new
                {
                    task_id = table.Column<int>(type: "int", nullable: false),
                    label_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TaskLabe__9AD6EB28B3BC91DC", x => new { x.task_id, x.label_id });
                    table.ForeignKey(
                        name: "FK__TaskLabel__label__02FC7413",
                        column: x => x.label_id,
                        principalTable: "Labels",
                        principalColumn: "label_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__TaskLabel__task___02084FDA",
                        column: x => x.task_id,
                        principalTable: "Tasks",
                        principalColumn: "task_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    log_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    event_id = table.Column<int>(type: "int", nullable: true),
                    action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    action_time = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AuditLog__9E2397E00216AADF", x => x.log_id);
                    table.ForeignKey(
                        name: "FK__AuditLogs__event__72C60C4A",
                        column: x => x.event_id,
                        principalTable: "Events",
                        principalColumn: "event_id");
                    table.ForeignKey(
                        name: "FK__AuditLogs__user___71D1E811",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "EventShares",
                columns: table => new
                {
                    share_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    event_id = table.Column<int>(type: "int", nullable: false),
                    shared_with_email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    shared_with_user_id = table.Column<int>(type: "int", nullable: true),
                    permission = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "view"),
                    shared_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EventSha__C36E8AE5F657CFD5", x => x.share_id);
                    table.ForeignKey(
                        name: "FK__EventShar__event__6754599E",
                        column: x => x.event_id,
                        principalTable: "Events",
                        principalColumn: "event_id");
                    table.ForeignKey(
                        name: "FK__EventShar__share__68487DD7",
                        column: x => x.shared_with_user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    notification_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    event_id = table.Column<int>(type: "int", nullable: false),
                    notify_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    notify_method = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "email"),
                    status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "pending"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__E059842F16107B95", x => x.notification_id);
                    table.ForeignKey(
                        name: "FK__Notificat__event__619B8048",
                        column: x => x.event_id,
                        principalTable: "Events",
                        principalColumn: "event_id");
                    table.ForeignKey(
                        name: "FK__Notificat__user___60A75C0F",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_event_id",
                table: "AuditLogs",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_user_id",
                table: "AuditLogs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarSyncs_user_id",
                table: "CalendarSyncs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_user_id",
                table: "Categories",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Events_category_id",
                table: "Events",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Events_user_id",
                table: "Events",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_EventShares_event_id",
                table: "EventShares",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_EventShares_shared_with_user_id",
                table: "EventShares",
                column: "shared_with_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_event_id",
                table: "Notifications",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_user_id",
                table: "Notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLabels_label_id",
                table: "TaskLabels",
                column: "label_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_user_id",
                table: "Tasks",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__AB6E6164955854E7",
                table: "Users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "CalendarSyncs");

            migrationBuilder.DropTable(
                name: "EventShares");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "TaskLabels");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Labels");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
