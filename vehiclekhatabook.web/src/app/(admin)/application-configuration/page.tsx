'use client';
import React, { useState, useEffect } from 'react';
import { Form, FormGroup, Label, Input, Button, Row, Col, Table } from 'reactstrap';
import { addApplicationConfiguration, getApplicationConfiguration, updateApplicationConfiguration } from '@/service/admin.service';
import { Switch } from '@mui/material';
import { v4 as uuidv4 } from 'uuid';

type ConfigurationItem = {
    fieldName: '';
    label: '';
    value: '' | 0 | boolean | null;
    applicationConfigurationId?: '0'; 
};

type FormData = Record<string, string | number | boolean | null>;

const ConfigurationPage = () => {
    const [configuration, setConfiguration] = useState<ConfigurationItem[]>([]);
    const [formData, setFormData] = useState<FormData>({});
    const [currentPage, setCurrentPage] = useState(1);
    const [successMessage, setSuccessMessage] = useState('');
    const [pageSize] = useState(5);
    const [isEditMode, setIsEditMode] = useState(false);

    useEffect(() => {
        fetchConfigurationData();
    }, []);

    const fetchConfigurationData = async () => {
        try {
            const response = await getApplicationConfiguration();
            if (response != null) {
                setIsEditMode(true);
            }
            else {
                setIsEditMode(false);
            }
            setConfiguration(response);
            const initialFormData: FormData = response.reduce((acc: any, item: ConfigurationItem) => {
                acc[item.fieldName] = item.value ?? '';
                return acc;
            }, {});
            setFormData(initialFormData);
        } catch (error) {
            console.error('Error fetching configuration data:', error);
        }
    };
    const resetForm = () => {
        setFormData(
            configuration.reduce((acc: any, item: ConfigurationItem) => {
                acc[item.fieldName] = '';
                return acc;
            }, {})
        );
        setIsEditMode(false);
        setSuccessMessage('');
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value, type, checked } = e.target;
        setFormData((prevFormData) => ({
            ...prevFormData,
            [name]: type === 'checkbox' ? checked : value,
        }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        debugger
        e.preventDefault();
        try {
            let payload: Record<string, any> = {};
            for (const item of configuration) {
                payload[item.fieldName] = formData[item.fieldName] ?? item.value;
            }
            for (const item of currentConfiguration) {
                const value = formData[item.fieldName] !== undefined ? formData[item.fieldName] : null;
                payload[item.fieldName] = value;
            }

            if (!isEditMode) {
                if ('ApplicationConfigurationId' in payload) {
                    payload['ApplicationConfigurationId'] = uuidv4();
                }
            }
            const { CreatedOn, CreatedBy, ModifiedBy, LastModifiedOn, ...payloadForAdd } = payload;

            if (isEditMode) {
                await updateApplicationConfiguration(payloadForAdd);
                setSuccessMessage('Configuration updated successfully!');
            } else {
                await addApplicationConfiguration(payloadForAdd);
                setSuccessMessage('Configuration added successfully!');
            }

            setTimeout(() => setSuccessMessage(''), 3000);
        } catch (error) {
            console.error('Error in handleSubmit:', error);
        }
    };
    const handlePaginationChange = (newPage: number) => {
        setCurrentPage(newPage);
    };
    const handleRadioChange = (e: React.ChangeEvent<HTMLInputElement>, fieldName: string) => {
        // Get the string value of the radio button
        const value = e.target.value; // Either "1" or "0"
    
        // Update formData with the string value ("1" or "0")
        setFormData((prevData) => ({
            ...prevData,
            [fieldName]: value,  // Set the value to "1" or "0"
        }));
    };
    
    const totalPages = Math.ceil(configuration.length / pageSize);
    const currentConfiguration = configuration;
    const tableHeaders = Object.keys(configuration[0] || {}).filter(key => key !== 'applicationConfigurationId');

    return (
        <div>
            <h3>Application Configuration</h3>
{successMessage && <div className="alert alert-success">{successMessage}</div>}
<Form onSubmit={handleSubmit}>
    <Row>
        {currentConfiguration
            .filter(
                (item) =>
                    ![
                        'ApplicationConfigurationId',
                        'CreatedOn',
                        'CreatedBy',
                        'ModifiedBy',
                        'LastModifiedOn',
                        'IsActive',
                    ].includes(item.fieldName)
            )
            .map((item, index) => (
                <Col md={6} key={item.fieldName}>
                    <FormGroup>
                        <Label for={item.fieldName}>{item.label}</Label>
                        {/* Check if fieldName is PaymentGatewayStatus or SubscriptionIsRenewable */}
                        {['PaymentGatewayStatus', 'SubscriptionIsRenewable'].includes(item.fieldName) ? (
                            // Render radio buttons for both fields
                            <div>
                                <Input
                                    type="radio"
                                    name={item.fieldName}
                                    id={`${item.fieldName}_true`}
                                    value="1" // Send "1" for true
                                    checked={formData[item.fieldName] === "1"} // Compare with "1"
                                    onChange={(e) => handleRadioChange(e, item.fieldName)}
                                />
                                <Label for={`${item.fieldName}_true`}>Yes</Label>

                                <Input
                                    type="radio"
                                    name={item.fieldName}
                                    id={`${item.fieldName}_false`}
                                    value="0" // Send "0" for false
                                    checked={formData[item.fieldName] === "0"} // Compare with "0"
                                    onChange={(e) => handleRadioChange(e, item.fieldName)}
                                />
                                <Label for={`${item.fieldName}_false`}>No</Label>
                            </div>
                        ) : (
                            // Default rendering for other fields
                            <Input
                                type={typeof item.value === 'boolean' ? 'checkbox' : 'text'}
                                name={item.fieldName}
                                id={item.fieldName}
                                value={
                                    typeof item.value === 'boolean'
                                        ? undefined
                                        : (formData[item.fieldName] as string | number)
                                }
                                checked={
                                    typeof item.value === 'boolean'
                                        ? (formData[item.fieldName] as boolean)
                                        : undefined
                                }
                                onChange={handleChange}
                            />
                        )}
                    </FormGroup>
                </Col>
            ))}
    </Row>
    <Button
        color="primary"
        type="submit"
        style={{ backgroundColor: "#F3AB3C", borderColor: "#F3AB3C" }}
    >
        {isEditMode ? "Update Configuration" : "Add Configuration"}
    </Button>
</Form>

        </div>

    );

};

export default ConfigurationPage;

