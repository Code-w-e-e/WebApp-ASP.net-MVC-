-- create database
CREATE DATABASE Authorization OWNER root;

-- ������������
create table Users
    (
        id              serial          Primary Key,
        fio             varchar(50)     not null,	
        login           varchar(20)     not null,
        password        varchar(20)     not null,
        role            varchar(20)     not null,	
		department      varchar(50)     not null,	
		part            varchar(20)     not null	
    );
insert into Users
            (fio, login, password, role, department, part) 
    values
            ('������� ����� ����������',   'user1', 'user1', 'admin', '����� ����������','�����������');
			
-- ��������
create table Abonents
    (
        id              serial          Primary Key,
        fio             varchar(50)     not null,	
        numcontract     int4            not null,
        number          varchar(20)     not null
    );
insert into Abonents
            (fio, numcontract, number) 
    values
            ('������ ���� ���������', 1546, '+79743613740'),
			('������ ����� ����������', 457,'+79502659248'),
			('����� ������� ���������', 1654,'+74587898854'),
			('��������� ������� ���������', 135,'86547894125'),
			('������� ��������� ����������', 1257,'86541239657'),
			('��������� ������ ���������', 644,'+74569514262');
	
-- �������
create table History
    (
        id              serial          Primary Key,
        fio             varchar(50)     not null,	
		department      varchar(50)     not null,	
		part            varchar(20)     not null,
        date            timestamp       not null		
    );
	
-- �����������
create table Certificates
    (
        id                serial          Primary Key,
        numcertificate    varchar(20)     not null,
        dateStart         timestamp       not null,
		dateend           timestamp       null
    );
insert into Certificates
            (numcertificate, dateStart, dateend) 
    values
            ('a45-8957', '2020-02-01 00:00:00', '2021-05-04 00:00:00'),
			('f855-756', '2022-12-01 00:00:00'),
			('j025-88', '2020-05-10 00:00:00', '2021-08-08 00:00:00'),
			('k555-858', '2022-02-21 00:00:00'),
			('028-77', '2020-02-11 00:00:00', '2022-06-04 00:00:00'),
			('465-ds45', '2021-02-01 00:00:00', '2022-05-14 00:00:00');