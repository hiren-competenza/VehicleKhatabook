'use client';
import React, { useState, useEffect } from 'react';
import { Form, FormGroup, Label, Input, Button, Row, Col, Table } from 'reactstrap';
import { addApplicationConfiguration, getApplicationConfiguration, updateApplicationConfiguration } from '@/service/admin.service';
import { Switch } from '@mui/material';

const ConfigurationPage = () => {
    const [configData, setConfigData] = useState({
        applicationConfigurationId: '0',
        smsApiKey: '',
        smsApiUrl: '',
        smsSenderId: '',
        supportEmail: '',
        supportWhatsAppNumber: '',
        paymentGatewayApiKey: '',
        paymentGatewayPublicKey: '',
        paymentGatewayWebhookSecret: '',
        paymentGatewayCurrency: '',
        paymentGatewayName: '',
        paymentGatewayStatus: true,
        subscriptionName: '',
        subscriptionAmount: 0,
        subscriptionDurationDays: 0,
        isRenewable: true,
        renewalReminderDaysBefore: 0,
        trialPeriodDays: 0,
        facebookPageUrl: '',
        twitterHandle: '',
        instagramHandle: '',
        linkedInUrl: '',
        youtubeChannelUrl: '',
        pinterestUrl: '',
        isActive: true,
    });
    const [configuration, setConfiguration] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [isEditMode, setIsEditMode] = useState(false);
    const [successMessage, setSuccessMessage] = useState('');
    const [pageSize] = useState(5);

    useEffect(() => {
        fetchConfigurationData();
    }, []);

    const handleChange = (e:any) => {
        const { name, value, type, checked } = e.target;
        setConfigData((prevData) => ({
            ...prevData,
            [name]: type === 'checkbox' ? checked : value,
        }));
    };

    const handleSubmit = async (e: any) => {
        e.preventDefault();
    
        try {
            let payload;
    
            if (isEditMode) {
                // For edit mode, use the full configuration object
                payload = { ...configData };
                await updateApplicationConfiguration(payload);
            } else {
                // For add mode, exclude the applicationConfigurationId
                const { applicationConfigurationId, ...rest } = configData; // Exclude the ID
                payload = rest;
                await addApplicationConfiguration(payload);
            }
    
            setSuccessMessage(isEditMode ? 'Configuration updated successfully!' : 'Configuration added successfully!');
            fetchConfigurationData();
            resetForm();
            setTimeout(() => setSuccessMessage(""), 3000);
        } catch (error) {
            console.error('Error submitting form:', error);
        }
    };
    
    
    const fetchConfigurationData = async () => {
        try {
            const data = await getApplicationConfiguration();
            setConfiguration(data);
        } catch (error) {
            console.error("Error fetching data:", error);
        }
    };

    const resetForm = () => {
        setConfigData({
            applicationConfigurationId: '0',
            smsApiKey: '',
            smsApiUrl: '',
            smsSenderId: '',
            supportEmail: '',
            supportWhatsAppNumber: '',
            paymentGatewayApiKey: '',
            paymentGatewayPublicKey: '',
            paymentGatewayWebhookSecret: '',
            paymentGatewayCurrency: '',
            paymentGatewayName: '',
            paymentGatewayStatus: true,
            subscriptionName: '',
            subscriptionAmount: 0,
            subscriptionDurationDays: 0,
            isRenewable: true,
            renewalReminderDaysBefore: 0,
            trialPeriodDays: 0,
            facebookPageUrl: '',
            twitterHandle: '',
            instagramHandle: '',
            linkedInUrl: '',
            youtubeChannelUrl: '',
            pinterestUrl: '',
            isActive: true,
        });
        setIsEditMode(false);
    };

    const handlePaginationChange = (newPage:any) => {
        setCurrentPage(newPage);
    };
    const totalPages = Math.ceil(configuration.length / pageSize);

    const currentConfiguration = configuration.slice(
        (currentPage - 1) * pageSize,
        currentPage * pageSize
    );

    const handleEdit = (config:any) => {
        setIsEditMode(true);
        setConfigData({
            applicationConfigurationId: config.applicationConfigurationId,
            smsApiKey: config.smsApiKey,
            smsApiUrl: config.smsApiUrl,
            smsSenderId: config.smsSenderId,
            supportEmail: config.supportEmail,
            supportWhatsAppNumber: config.supportWhatsAppNumber,
            paymentGatewayApiKey: config.paymentGatewayApiKey,
            paymentGatewayPublicKey: config.paymentGatewayPublicKey,
            paymentGatewayWebhookSecret: config.paymentGatewayWebhookSecret,
            paymentGatewayCurrency: config.paymentGatewayCurrency,
            paymentGatewayName: config.paymentGatewayName,
            paymentGatewayStatus: config.paymentGatewayStatus,
            subscriptionName: config.subscriptionName,
            subscriptionAmount: config.subscriptionAmount,
            subscriptionDurationDays: config.subscriptionDurationDays,
            isRenewable: config.isRenewable,
            renewalReminderDaysBefore: config.renewalReminderDaysBefore,
            trialPeriodDays: config.trialPeriodDays,
            facebookPageUrl: config.facebookPageUrl,
            twitterHandle: config.twitterHandle,
            instagramHandle: config.instagramHandle,
            linkedInUrl: config.linkedInUrl,
            youtubeChannelUrl: config.youtubeChannelUrl,
            pinterestUrl: config.pinterestUrl,
            isActive: config.isActive,
        });
    };

    return (
        <div className="mt-4">
            <h3>Application Configuration</h3>
            <Form onSubmit={handleSubmit}>
                <Row>
                    {/* SMS API Configuration */}
                    <Col md={6}>
                        <FormGroup>
                            <Label for="smsApiKey">SMS API Key</Label>
                            <Input
                                type="text"
                                name="smsApiKey"
                                id="smsApiKey"
                                value={configData.smsApiKey || ''}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="smsApiUrl">SMS API URL</Label>
                            <Input
                                type="url"
                                name="smsApiUrl"
                                id="smsApiUrl"
                                value={configData.smsApiUrl || ''}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="smsSenderId">SMS Sender ID</Label>
                            <Input
                                type="text"
                                name="smsSenderId"
                                id="smsSenderId"
                                value={configData.smsSenderId || ''}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>

                    {/* Contact Information */}
                    <Col md={6}>
                        <FormGroup>
                            <Label for="supportEmail">Support Email</Label>
                            <Input
                                type="email"
                                name="supportEmail"
                                id="supportEmail"
                                value={configData.supportEmail || ''}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="supportWhatsAppNumber">Support WhatsApp Number</Label>
                            <Input
                                type="text"
                                name="supportWhatsAppNumber"
                                id="supportWhatsAppNumber"
                                value={configData.supportWhatsAppNumber || ''}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>

                    {/* Payment Gateway Configuration */}
                    <Col md={6}>
                        <FormGroup>
                            <Label for="paymentGatewayApiKey">Payment Gateway API Key</Label>
                            <Input
                                type="text"
                                name="paymentGatewayApiKey"
                                id="paymentGatewayApiKey"
                                value={configData.paymentGatewayApiKey || ''}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="paymentGatewayPublicKey">Payment Gateway Public Key</Label>
                            <Input
                                type="text"
                                name="paymentGatewayPublicKey"
                                id="paymentGatewayPublicKey"
                                value={configData.paymentGatewayPublicKey || ''}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="paymentGatewayWebhookSecret">Payment Gateway Webhook Secret</Label>
                            <Input
                                type="text"
                                name="paymentGatewayWebhookSecret"
                                id="paymentGatewayWebhookSecret"
                                value={configData.paymentGatewayWebhookSecret || ''}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="paymentGatewayCurrency">Payment Gateway Currency</Label>
                            <Input
                                type="text"
                                name="paymentGatewayCurrency"
                                id="paymentGatewayCurrency"
                                value={configData.paymentGatewayCurrency || ''}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="paymentGatewayName">Payment Gateway Name</Label>
                            <Input
                                type="text"
                                name="paymentGatewayName"
                                id="paymentGatewayName"
                                value={configData.paymentGatewayName || ''}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label>Payment Gateway Status</Label>
                            <Switch
                                checked={configData.paymentGatewayStatus }
                                name=" paymentGatewayStatus"
                                color="primary"
                                onChange={(e) =>
                                    setConfigData({
                                        ...configData,
                                        paymentGatewayStatus: e.target.checked,
                                    })
                                }
                            />
                        </FormGroup>
                    </Col>

                    {/* Subscription Configuration */}
                    <Col md={6}>
                        <FormGroup>
                            <Label for="subscriptionName">Subscription Name</Label>
                            <Input
                                type="text"
                                name="subscriptionName"
                                id="subscriptionName"
                                value={configData.subscriptionName}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="subscriptionAmount">Subscription Amount</Label>
                            <Input
                                type="number"
                                name="subscriptionAmount"
                                id="subscriptionAmount"
                                value={configData.subscriptionAmount}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="subscriptionDurationDays">Subscription Duration (Days)</Label>
                            <Input
                                type="number"
                                name="subscriptionDurationDays"
                                id="subscriptionDurationDays"
                                value={configData.subscriptionDurationDays}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label>Is Renewable</Label>
                            <Switch
                                checked={configData.isRenewable}
                                name="isRenewable"
                                color="primary"
                                onChange={(e) =>
                                    setConfigData({
                                        ...configData,
                                        isRenewable: e.target.checked,
                                    })
                                }
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="renewalReminderDaysBefore">Renewal Reminder (Days Before)</Label>
                            <Input
                                type="number"
                                name="renewalReminderDaysBefore"
                                id="renewalReminderDaysBefore"
                                value={configData.renewalReminderDaysBefore}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="trialPeriodDays">Trial Period (Days)</Label>
                            <Input
                                type="number"
                                name="trialPeriodDays"
                                id="trialPeriodDays"
                                value={configData.trialPeriodDays}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>

                    {/* Social Media Links */}
                    <Col md={6}>
                        <FormGroup>
                            <Label for="facebookPageUrl">Facebook Page URL</Label>
                            <Input
                                type="url"
                                name="facebookPageUrl"
                                id="facebookPageUrl"
                                value={configData.facebookPageUrl}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="twitterHandle">Twitter Handle</Label>
                            <Input
                                type="text"
                                name="twitterHandle"
                                id="twitterHandle"
                                value={configData.twitterHandle}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="instagramHandle">Instagram Handle</Label>
                            <Input
                                type="text"
                                name="instagramHandle"
                                id="instagramHandle"
                                value={configData.instagramHandle}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="linkedInUrl">LinkedIn URL</Label>
                            <Input
                                type="url"
                                name="linkedInUrl"
                                id="linkedInUrl"
                                value={configData.linkedInUrl}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="youtubeChannelUrl">YouTube Channel URL</Label>
                            <Input
                                type="url"
                                name="youtubeChannelUrl"
                                id="youtubeChannelUrl"
                                value={configData.youtubeChannelUrl}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="pinterestUrl">Pinterest URL</Label>
                            <Input
                                type="url"
                                name="pinterestUrl"
                                id="pinterestUrl"
                                value={configData.pinterestUrl}
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>

                    {/* Active Switch */}
                    
                    <Col md={6}>
                        <FormGroup>
                            <Label>Is Active</Label>
                            <Switch
                                checked={configData.isActive}
                                name="isActive"
                                color="primary"
                                onChange={(e) =>
                                    setConfigData({
                                        ...configData,
                                        isActive: e.target.checked,
                                    })
                                }
                            />
                        </FormGroup>
                    </Col>
                    {successMessage && <div className="alert alert-success">{successMessage}</div>}
                    <Col md={12}>
                    <div className="button-group d-flex flex-column flex-sm-row">
                    <Button
                        color="primary"
                        type="submit"
                        style={{ backgroundColor: "#F3AB3C", borderColor: "#F3AB3C" }}
                    >
                            {isEditMode ? 'Update Configuration' : 'Add Configuration'}
                            </Button>

                    {isEditMode && (
                        <Button
                            color="secondary"
                            onClick={resetForm}
                            className="ms-2"
                            style={{ backgroundColor: "#F3AB3C", borderColor: "#F3AB3C" }}
                        >
                            Cancel
                        </Button>
                    )}
                </div>
                    </Col>
                </Row>
            </Form>

            <div>
                <Row className="mt-4">
                    <Col md={12}>
                        <h4>Configuration Data</h4>
                        <Table bordered>
                            <thead>
                                <tr>
                                    <th>SMS API Key</th>
                                    <th>SMS API URL</th>
                                    <th>SMS Sender ID</th>
                                    <th>Support Email</th>
                                    <th>Support WhatsApp Number</th>
                                    <th>Payment Gateway Name</th>
                                    <th>Payment Gateway Status</th>
                                    <th>Is Active</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {currentConfiguration.length > 0 ? (
                                    currentConfiguration.map((config:any) => (
                                        <tr key={config.applicationConfigurationId}>
                                            <td>{config.smsApiKey}</td>
                                            <td>{config.smsApiUrl}</td>
                                            <td>{config.smsSenderId}</td>
                                            <td>{config.supportEmail}</td>
                                            <td>{config.supportWhatsAppNumber}</td>
                                            <td>{config.paymentGatewayName}</td>
                                            <td>{config.paymentGatewayStatus ? 'Active' : 'Inactive'}</td>
                                            <td>{config.isActive ? 'Active' : 'Inactive'}</td>
                                            <td>
                                                <Button
                                                    onClick={() => handleEdit(config)}
                                                    size="sm"
                                                    style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}
                                                >
                                                    Edit
                                                </Button>
                                            </td>
                                        </tr>
                                    ))
                                ) : (
                                    <tr>
                                        <td colSpan={9}>No data available</td>
                                    </tr>
                                )}
                            </tbody>
                        </Table>
                        <div className="pagination">
                            <Button
                                disabled={currentPage === 1}
                                onClick={() => handlePaginationChange(currentPage - 1)}
                                style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}

                            >
                                Previous
                            </Button>
                            <span>{`Page ${currentPage} of ${totalPages}`}</span>
                            <Button
                                disabled={currentPage === totalPages}
                                onClick={() => handlePaginationChange(currentPage + 1)}
                                style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}

                            >
                                Next
                            </Button>
                        </div>
                    </Col>
                </Row>
            </div>
        </div>
    );
};

export default ConfigurationPage;