ALTER TABLE surpathlive.individual_pids
 ADD validated BIT NOT NULL DEFAULT b'0' AFTER mask_pid;
