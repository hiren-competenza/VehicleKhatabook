'use client';
import React, { useState, useEffect } from 'react';
import { Form, FormGroup, Label, Input, Button, Row, Col } from 'reactstrap';
import { addSmsConfiguration, getSmsConfiguration, updateSmsConfiguration } from '@/service/admin.service';
import { Switch } from '@mui/material';

const SmsConfigurationPage = () => {
    const [smsConfigData, setSmsConfigData] = useState({
        providerID: 0,
        providerName: "",
        apiUrl: "",
        authKey: "",
        clientID: "",
        senderID: "",
        timeout: 0,
        isActive: true,
    });
    const [isEditMode, setIsEditMode] = useState(false);
    const [successMessage, setSuccessMessage] = useState(""); // State for success message

    // Fetch SMS Configuration on component mount
    useEffect(() => {
        fetchSmsConfigurationData();
    }, []);
    
    const handleChange = (e: any) => {
        const { name, value, type, checked } = e.target;
        setSmsConfigData((prevData) => ({
            ...prevData,
            [name]: type === "checkbox" ? checked : value,
        }));
    };

    const handleSubmit = async (e: any) => {
        e.preventDefault();
        try {
            if (isEditMode) {
                await updateSmsConfiguration(smsConfigData);
            } else {
                await addSmsConfiguration(smsConfigData);
            }
            setSuccessMessage(isEditMode ? "SMS configuration updated successfully!" : "SMS configuration added successfully!");
            setIsEditMode(false);
            fetchSmsConfigurationData(); // Refresh the data after submission

            // Clear success message after 3 seconds
            setTimeout(() => setSuccessMessage(""), 3000);
        } catch (error) {
            console.error("Error updating SMS configuration data:", error);
        }
    };

    const fetchSmsConfigurationData = async () => {
        try {
            const data = await getSmsConfiguration();
            if (data && data.length > 0) {
                // Extract the first item from the array and set it to smsConfigData
                setIsEditMode(true);
                setSmsConfigData(data[0]);
            } else {
                // No data found, show form for new entry
                setIsEditMode(false);
                setSmsConfigData({
                    providerID: 0,
                    providerName: "",
                    apiUrl: "",
                    authKey: "",
                    clientID: "",
                    senderID: "",
                    timeout: 0,
                    isActive: true,
                });
            }
        } catch (error) {
            console.error("Error fetching SMS configuration data:", error);
        }
    };

    const resetForm = () => {
        setSmsConfigData({
            providerID: 0,
            providerName: "",
            apiUrl: "",
            authKey: "",
            clientID: "",
            senderID: "",
            timeout: 0,
            isActive: true,
        });
        setIsEditMode(false);
    };

    const handleCancel = () => {
        resetForm();
    };

    return (
        <div className=" mt-4">
         <h3>SMS Configuration</h3>

            <Form onSubmit={handleSubmit} className="sms-configuration-form">

                <Row form>
                    <Col xs={12} md={6}>
                        <FormGroup>
                            <Label for="providerName">Provider Name</Label>
                            <Input
                                type="text"
                                name="providerName"
                                id="providerName"
                                placeholder="Enter provider name"
                                value={smsConfigData.providerName || ""}
                                onChange={handleChange}
                                required
                            />
                        </FormGroup>
                    </Col>
                    <Col xs={12} md={6}>
                        <FormGroup>
                            <Label for="apiUrl">API URL</Label>
                            <Input
                                type="url"
                                name="apiUrl"
                                id="apiUrl"
                                placeholder="Enter API URL"
                                value={smsConfigData.apiUrl || ""}
                                onChange={handleChange}
                                required
                            />
                        </FormGroup>
                    </Col>
                    <Col xs={12} md={6}>
                        <FormGroup>
                            <Label for="authKey">Auth Key</Label>
                            <Input
                                type="text"
                                name="authKey"
                                id="authKey"
                                placeholder="Enter authentication key"
                                value={smsConfigData.authKey || ""}
                                onChange={handleChange}
                                required
                            />
                        </FormGroup>
                    </Col>
                    <Col xs={12} md={6}>
                        <FormGroup>
                            <Label for="clientID">Client ID</Label>
                            <Input
                                type="text"
                                name="clientID"
                                id="clientID"
                                placeholder="Enter client ID"
                                value={smsConfigData.clientID || ""}
                                onChange={handleChange}
                                required
                            />
                        </FormGroup>
                    </Col>
                    <Col xs={12} md={6}>
                        <FormGroup>
                            <Label for="senderID">Sender ID</Label>
                            <Input
                                type="text"
                                name="senderID"
                                id="senderID"
                                placeholder="Enter sender ID"
                                value={smsConfigData.senderID || ""}
                                onChange={handleChange}
                                required
                            />
                        </FormGroup>
                    </Col>
                    <Col xs={12} md={6}>
                        <FormGroup>
                            <Label for="timeout">Timeout</Label>
                            <Input
                                type="number"
                                name="timeout"
                                id="timeout"
                                placeholder="Enter timeout"
                                value={smsConfigData.timeout || ""}
                                onChange={handleChange}
                                required
                            />
                        </FormGroup>
                    </Col>
                    <Col xs={12} md={6}>
                        <FormGroup>
                            <Label>Is Active</Label>
                            <Switch
                                checked={smsConfigData.isActive}
                                name="isActive"
                                color="primary"
                                onChange={(e) => setSmsConfigData({ ...smsConfigData, isActive: e.target.checked })}
                            />
                        </FormGroup>
                    </Col>
                </Row>

                <div className="button-group d-flex flex-column flex-sm-row">
                    <Button color="primary" type="submit" className="submit-button mb-3 mb-sm-0"
                    style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}>
                        {isEditMode ? "Update" : "Submit"}
                        
                    </Button>
                    {/* <Button color="secondary" onClick={handleCancel} className="mt-2 mt-sm-0 ms-sm-2">
                        Cancel
                    </Button> */}
                </div>

                {/* Display success message */}
                {successMessage && <div className="alert alert-success mt-3">{successMessage}</div>}
            </Form>
        </div>
    );
};

export default SmsConfigurationPage;
