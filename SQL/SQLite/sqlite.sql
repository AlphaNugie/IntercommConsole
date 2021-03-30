CREATE TABLE [t_radar_distances_time](
  [radar_id] INTEGER NOT NULL ON CONFLICT FAIL, 
  [radar_name] VARCHAR2(40), 
  [distance] NUMBER NOT NULL ON CONFLICT FAIL DEFAULT 0, 
  [time] DATETIME DEFAULT (DATETIME ('now', 'localtime')));

CREATE TABLE [t_radar_distances_his](
  [record_id] INTEGER PRIMARY KEY ON CONFLICT FAIL AUTOINCREMENT NOT NULL ON CONFLICT FAIL, 
  [tipx] NUMBER NOT NULL ON CONFLICT FAIL DEFAULT 0, 
  [tipy] NUMBER NOT NULL ON CONFLICT FAIL DEFAULT 0, 
  [tipz] NUMBER NOT NULL ON CONFLICT FAIL DEFAULT 0, 
  [radar_left] NUMBER NOT NULL ON CONFLICT FAIL DEFAULT 0, 
  [radar_right] NUMBER NOT NULL ON CONFLICT FAIL DEFAULT 0, 
  [stack_dist] NUMBER, 
  [time] DATETIME DEFAULT (DATETIME ('now', 'localtime')));
  
CREATE TABLE [t_plc_opcgroup](
  [group_id] INTEGER PRIMARY KEY ON CONFLICT FAIL AUTOINCREMENT NOT NULL ON CONFLICT FAIL, 
  [group_name] VARCHAR2(32) NOT NULL ON CONFLICT FAIL, 
  [group_type] INTEGER(1) NOT NULL ON CONFLICT FAIL DEFAULT 1);

CREATE TABLE [t_plc_opcitem](
  [record_id] INTEGER PRIMARY KEY ON CONFLICT FAIL AUTOINCREMENT NOT NULL ON CONFLICT FAIL, 
  [item_id] VARCHAR2(64) NOT NULL ON CONFLICT FAIL, 
  [opcgroup_id] INTEGER NOT NULL ON CONFLICT FAIL, 
  [field_name] VARCHAR2(64)), 
  [enabled] INTEGER NOT NULL ON CONFLICT FAIL DEFAULT 1);

insert into t_plc_opcgroup (group_name, group_type) values('OPC_GROUP_READ', 1);
insert into t_plc_opcgroup (group_name, group_type) values('OPC_GROUP_WRITE', 2);

--S1
INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[S1_TOPIC]WRHLongTravelLocationFromGPS', '2', 'WalkingPosition');
INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[S1_TOPIC]WRHBoomLuffAngleGPS', '2', 'PitchAngle');
INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[S1_TOPIC]WRHBoomSlewAngleGPS', '2', 'YawAngle');
INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[S1_TOPIC]WRH_Radar_Pile_Height', '2', 'PileHeight');
INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[S1_TOPIC]WRH_Radar_Pile_Height', '1', 'Pile');
INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[S1_TOPIC]ENCODER_DATA[2]', '1', 'WalkingPositionLeft_Plc');
INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[S1_TOPIC]ENCODER_DATA[3]', '1', 'WalkingPositionRight_Plc');
INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[S1_TOPIC]ENCODER_DATA[0]', '1', 'PitchAngle_Plc');
INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[S1_TOPIC]ENCODER_DATA[1]', '1', 'YawAngle_Plc');
--R1
--INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[R1_TOPIC]WRHLongTravelLocationFromGPS', '2', 'WalkingPosition');
--INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[R1_TOPIC]WRHBoomLuffAngleGPS', '2', 'PitchAngle');
--INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[R1_TOPIC]WRHBoomSlewAngleGPS', '2', 'YawAngle');
--INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[R1_TOPIC]WRH_Radar_Pile_Height', '2', 'PileHeight');
--INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[R1_TOPIC]WRH_Radar_Pile_Height', '1', 'Pile');
--INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[R1_TOPIC]ENCODER_DATA[2]', '1', 'WalkingPositionLeft_Plc');
--INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[R1_TOPIC]ENCODER_DATA[3]', '1', 'WalkingPositionRight_Plc');
--INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[R1_TOPIC]ENCODER_DATA[0]', '1', 'PitchAngle_Plc');
--INSERT INTO "main"."t_plc_opcitem" ("item_id", "opcgroup_id", "field_name") VALUES ('[R1_TOPIC]ENCODER_DATA[1]', '1', 'YawAngle_Plc');

select * from t_radar_distances_his t where t.time between datetime('2020-05-20 02:30:00') and datetime('2020-05-20 18:30:00') order by t.time desc