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
  [field_name] VARCHAR2(64));

insert into t_plc_opcgroup (group_name, group_type) values('OPC_GROUP_READ', 1);
insert into t_plc_opcgroup (group_name, group_type) values('OPC_GROUP_WRITE', 2);

select * from t_radar_distances_his t where t.time between datetime('2020-05-20 02:30:00') and datetime('2020-05-20 18:30:00') order by t.time desc