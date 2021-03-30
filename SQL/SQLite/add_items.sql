--自动取料/左右点云出垛边角度
insert into t_plc_opcitem (item_id, opcgroup_id, field_name) values ('[R1_TOPIC]Rc_AutoControlOrderType.6', 1, 'AutoControl');
insert into t_plc_opcitem (item_id, opcgroup_id, field_name) values ('[R1_TOPIC]Rc_SlewTargetAngle[0]', 1, 'ModelBoundAngle1');
insert into t_plc_opcitem (item_id, opcgroup_id, field_name) values ('[R1_TOPIC]Rc_SlewTargetAngle[1]', 1, 'ModelBoundAngle2');
--臂架左前中后、右前中后报警级别
insert into t_plc_opcitem (item_id, opcgroup_id, field_name) values ('[R1_TOPIC]WRH_Coll_ArmLeftFrontAlarm', 2, 'LevelLeftFront');
insert into t_plc_opcitem (item_id, opcgroup_id, field_name) values ('[R1_TOPIC]WRH_Coll_ArmLeftMiddleAlarm', 2, 'LevelLeftMiddle');
insert into t_plc_opcitem (item_id, opcgroup_id, field_name) values ('[R1_TOPIC]WRH_Coll_ArmLeftBackAlarm', 2, 'LevelLeftBack');
insert into t_plc_opcitem (item_id, opcgroup_id, field_name) values ('[R1_TOPIC]WRH_Coll_ArmRightFrontAlarm', 2, 'LevelRightFront');
insert into t_plc_opcitem (item_id, opcgroup_id, field_name) values ('[R1_TOPIC]WRH_Coll_ArmRightMiddleAlarm', 2, 'LevelRightMiddle');
insert into t_plc_opcitem (item_id, opcgroup_id, field_name) values ('[R1_TOPIC]WRH_Coll_ArmRightBackAlarm', 2, 'LevelRightBack');