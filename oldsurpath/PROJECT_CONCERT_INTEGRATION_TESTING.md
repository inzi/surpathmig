# Project Concert Integration Testing Guide

## Overview

Project Concert integration enables seamless donor registration and automated result delivery between Surpath and Project Concert systems. This document outlines the integration architecture and provides testing procedures.

## Integration Architecture

### Components
1. **Handoff URL Generation** - Pre-populated donor registration
2. **RESTful API** - Data exchange and donor management
3. **SFTP Result Delivery** - Automated test result transmission
4. **Encryption** - TripleDES encryption for all data exchanges

### Data Flow

```
Project Concert → Surpath (Registration):
1. Generate encrypted handoff URL with donor data
2. Donor clicks URL → lands on Surpath registration
3. System validates and pre-populates form
4. Donor completes registration
5. System links donor via integration_id

Surpath → Project Concert (Results):
1. Test completed in Surpath
2. IntegrationPusher identifies completed tests
3. Generate encrypted result file
4. Push via SFTP to Project Concert
5. Track delivery status
```

## Testing Procedures

### 1. Test Handoff URL Generation

**Endpoint**: Generate handoff URL for test donor

**Test Data Structure**:
```json
{
  "partner_client_code": "CONCERT-TEST-001",
  "integration_id": "TEST-DONOR-12345",
  "donor_first_name": "John",
  "donor_last_name": "TestDonor",
  "donor_email": "testdonor@projectconcert.test",
  "donor_phone": "555-0123",
  "donor_ssn_last4": "9999",
  "donor_dob": "1990-01-01",
  "donor_address": "123 Test Street",
  "donor_city": "Test City",
  "donor_state": "TX",
  "donor_zip": "12345"
}
```

**Expected URL Format**:
```
https://[surpath-domain]/Registration/handoff/[partner_key]/[encrypted_dto]
```

### 2. Test API Integration

**Authentication Header**:
```
SurpathKey: [partner_key]
```

**Test Endpoints**:

#### a. Verify Partner Authentication
```
GET /api/integration/settings
```

#### b. Test Client Mapping
```
GET /api/integration/clients
```

#### c. Test Donor Retrieval
```
GET /api/integration/donors?integrationId=TEST-DONOR-12345
```

### 3. Test Result Transmission

**Methods to Transmit Test Data Back**:

#### Method 1: SFTP Push (Automated)
1. Complete a test for the registered donor
2. IntegrationPusher will automatically:
   - Generate result file
   - Encrypt using partner_crypto key
   - Push to configured SFTP location
   - File naming: `[client_folder]/[timestamp]_[donor_id].enc`

**Test Result File Structure**:
```json
{
  "integration_id": "TEST-DONOR-12345",
  "donor_id": "[surpath_donor_id]",
  "test_results": [
    {
      "test_type": "UA",
      "test_date": "2024-01-15",
      "result": "Negative",
      "substances": [],
      "collected_date": "2024-01-14",
      "reported_date": "2024-01-15"
    }
  ],
  "documents": [
    {
      "type": "COC",
      "filename": "COC_12345.pdf",
      "content_base64": "[base64_encoded_pdf]"
    }
  ]
}
```

#### Method 2: API Pull (On-Demand)
```
GET /api/integration/donors?integrationId=TEST-DONOR-12345&includeDocuments=true
```

Response includes:
- Donor information
- Test results
- Base64 encoded documents

#### Method 3: Manual Testing via Database
1. Insert test result for registered donor
2. Trigger IntegrationPusher manually
3. Monitor SFTP folder for encrypted file

### 4. Test Scenarios

#### Scenario 1: New Donor Registration
1. Generate handoff URL with test donor data
2. Access URL in browser
3. Verify pre-populated fields
4. Complete registration
5. Confirm integration_id linkage

#### Scenario 2: Result Delivery
1. Create test result for registered donor
2. Wait for IntegrationPusher cycle (or trigger manually)
3. Verify encrypted file in SFTP folder
4. Decrypt and validate content

#### Scenario 3: API Data Retrieval
1. Use API to query donor by integration_id
2. Verify returned data matches expected format
3. Test document retrieval with base64 encoding

### 5. Testing Configuration

**Required Partner Settings**:
```
partner_key: CONCERT-TEST
partner_crypto: [32-character encryption key]
sftp_host: sftp.projectconcert.test
sftp_username: surpath_test
sftp_password: [encrypted]
sftp_folder: /test_results/
```

## Troubleshooting

### Common Issues:
1. **Handoff URL Invalid**: Check encryption key and URL encoding
2. **API Authentication Failed**: Verify SurpathKey header
3. **SFTP Connection Failed**: Check credentials and firewall rules
4. **Result File Not Generated**: Verify test completion status
5. **Encryption/Decryption Error**: Ensure matching crypto keys

### Debug Logs:
- Handoff processing: Check web application logs
- API requests: Monitor IIS logs
- SFTP delivery: Review IntegrationPusher service logs
- Database: Check integration_* tables for status

## Security Considerations

1. Use unique encryption keys for test environment
2. Rotate test credentials regularly
3. Monitor failed authentication attempts
4. Validate all test data before production use
5. Ensure SFTP uses secure protocols (SFTP/FTPS)

## Contact for Testing Support

For integration testing assistance:
- Technical issues: Review IntegrationPusher logs
- API questions: Check IntegrationController implementation
- Encryption problems: Verify TripleDES configuration