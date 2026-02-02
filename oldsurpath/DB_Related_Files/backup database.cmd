# mysqldump -uroot -P -R surpath > surpath-$(date +%Y%m%d).sql
# mysqldump -h 10.94.0.220 -usurpath -pz24XrByS1 -routines surpathlive > D:\SQLBackups\surpathlive-$(date +%Y%m%d).sql

# https://aws-labs.com/smarter-faster-backups-restores-mysql-databases-mysqldump-tips/
# mysqldump -h 10.94.0.220 -usurpath -pz24XrByS1 -routines -extended-insert -quick surpathlive > D:\SQLBackups\surpathlive-$(date +%Y%m%d).sql

mysqldump -h 10.94.0.220 -usurpath -pz24XrByS1 -e --verbose --routines --triggers --single-transaction surpathlive > surpathlive-20200806.sql