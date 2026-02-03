/**
 * RecordCategoryRules - View Page
 *
 * Migrated from Surpath112: Areas/App/Views/RecordCategoryRules/ViewRecordCategoryRule.cshtml
 *
 * Features:
 * - Full-page read-only view
 * - Displays all RecordCategoryRule fields
 * - Shows warning status names
 */

import React, { useState, useEffect } from 'react';
import { Card, Descriptions, Spin, message, Button, Tag } from 'antd';
import { ArrowLeftOutlined, CheckCircleOutlined, CloseCircleOutlined } from '@ant-design/icons';
import { useNavigate, useParams } from 'react-router-dom';
import { L } from '@/lib/abpUtility';
import {
  recordCategoryRulesService,
  type GetRecordCategoryRuleForViewDto
} from '@/services/surpath/recordCategoryRules.service';

const ViewRecordCategoryRulePage: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();

  // ============================================================================
  // STATE
  // ============================================================================
  const [loading, setLoading] = useState(false);
  const [record, setRecord] = useState<GetRecordCategoryRuleForViewDto | null>(null);

  // ============================================================================
  // DATA FETCHING
  // ============================================================================

  useEffect(() => {
    if (id) {
      loadRecord();
    }
  }, [id]);

  const loadRecord = async () => {
    if (!id) return;

    setLoading(true);
    try {
      const result = await recordCategoryRulesService.getRecordCategoryRuleForView(id);
      setRecord(result);
    } catch (error: any) {
      message.error(error.message || L('AnErrorOccurred'));
      navigate('/app/compliance/record-category-rules');
    } finally {
      setLoading(false);
    }
  };

  // ============================================================================
  // RENDER HELPERS
  // ============================================================================

  const renderBoolean = (value: boolean) => {
    return value ? (
      <Tag icon={<CheckCircleOutlined />} color="success">
        {L('Yes')}
      </Tag>
    ) : (
      <Tag icon={<CloseCircleOutlined />} color="default">
        {L('No')}
      </Tag>
    );
  };

  // ============================================================================
  // RENDER
  // ============================================================================

  if (!id) {
    return null;
  }

  return (
    <div style={{ padding: '24px' }}>
      <Card
        title={L('RecordCategoryRuleDetails')}
        extra={
          <Button icon={<ArrowLeftOutlined />} onClick={() => navigate('/app/compliance/record-category-rules')}>
            {L('Back')}
          </Button>
        }
      >
        <Spin spinning={loading}>
          {record && (
            <Descriptions bordered column={2}>
              {/* Basic Information */}
              <Descriptions.Item label={L('Name')} span={2}>
                <strong>{record.recordCategoryRule.name}</strong>
              </Descriptions.Item>

              <Descriptions.Item label={L('Description')} span={2}>
                {record.recordCategoryRule.description || '-'}
              </Descriptions.Item>

              {/* Flags */}
              <Descriptions.Item label={L('Notify')}>
                {renderBoolean(record.recordCategoryRule.notify)}
              </Descriptions.Item>

              <Descriptions.Item label={L('Required')}>
                {renderBoolean(record.recordCategoryRule.required)}
              </Descriptions.Item>

              <Descriptions.Item label={L('Expires')}>
                {renderBoolean(record.recordCategoryRule.expires)}
              </Descriptions.Item>

              <Descriptions.Item label={L('IsSurpathOnly')}>
                {renderBoolean(record.recordCategoryRule.isSurpathOnly)}
              </Descriptions.Item>

              {/* Expiration Settings */}
              <Descriptions.Item label={L('ExpireInDays')} span={2}>
                {record.recordCategoryRule.expireInDays} {L('Days')}
              </Descriptions.Item>

              {record.recordCategoryRule.expires && (
                <Descriptions.Item label={L('ExpiredStatus')} span={2}>
                  {record.recordCategoryRule.expiredStatusName || '-'}
                </Descriptions.Item>
              )}

              {/* Warning Settings - First */}
              <Descriptions.Item label={L('WarnDaysBeforeFirst')}>
                {record.recordCategoryRule.warnDaysBeforeFirst} {L('Days')}
              </Descriptions.Item>

              <Descriptions.Item label={L('FirstWarnStatus')}>
                {record.recordCategoryRule.firstWarnStatusName || '-'}
              </Descriptions.Item>

              {/* Warning Settings - Second */}
              <Descriptions.Item label={L('WarnDaysBeforeSecond')}>
                {record.recordCategoryRule.warnDaysBeforeSecond} {L('Days')}
              </Descriptions.Item>

              <Descriptions.Item label={L('SecondWarnStatus')}>
                {record.recordCategoryRule.secondWarnStatusName || '-'}
              </Descriptions.Item>

              {/* Warning Settings - Final */}
              <Descriptions.Item label={L('WarnDaysBeforeFinal')}>
                {record.recordCategoryRule.warnDaysBeforeFinal} {L('Days')}
              </Descriptions.Item>

              <Descriptions.Item label={L('FinalWarnStatus')}>
                {record.recordCategoryRule.finalWarnStatusName || '-'}
              </Descriptions.Item>

              {/* MetaData */}
              <Descriptions.Item label={L('MetaData')} span={2}>
                {record.recordCategoryRule.metaData ? (
                  <pre style={{ margin: 0, whiteSpace: 'pre-wrap', wordWrap: 'break-word' }}>
                    {record.recordCategoryRule.metaData}
                  </pre>
                ) : (
                  '-'
                )}
              </Descriptions.Item>
            </Descriptions>
          )}
        </Spin>
      </Card>
    </div>
  );
};

export default ViewRecordCategoryRulePage;
