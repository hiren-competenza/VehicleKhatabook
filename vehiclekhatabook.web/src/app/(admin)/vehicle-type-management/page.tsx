"use client";
import React, { useState, useEffect } from 'react';
import { Form, FormGroup, Label, Input, Button, Row, Col, Table } from 'reactstrap';
import { Switch } from '@mui/material';
import { addVehicleType, getVehicleType, updateVehicleType } from '@/service/admin.service';

const Page = () => {
    const [vehicleTypeData, setVehicleTypeData] = useState({
        vehicleTypeId: 0,
        typeName: "",
        isActive: true,
    });
    const [vehicleTypes, setVehicleTypes] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState("");
    const [isEditMode, setIsEditMode] = useState(false);
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage] = useState(5); // Set the number of items per page

    const handleChange = (e: any) => {
        const { name, value, type, checked } = e.target;
        setVehicleTypeData((prevData) => ({
            ...prevData,
            [name]: type === "checkbox" ? checked : value,
        }));
    };

    const handleSubmit = async (e: any) => {
        e.preventDefault();
        try {
            if (isEditMode) {
                await updateVehicleType(vehicleTypeData);
            } else {
                await addVehicleType(vehicleTypeData);
            }
            setIsEditMode(false);
            setVehicleTypeData({
                vehicleTypeId: 0,
                typeName: "",
                isActive: false,
            });
            fetchVehicleTypeData();
        } catch (error) {
            console.error("Error saving vehicle type data:", error);
        }
    };

    const fetchVehicleTypeData = async () => {
        setIsLoading(true);
        setError("");
        try {
            const data = await getVehicleType();  // Assume this fetches all data
            setVehicleTypes(data);
        } catch (error) {
            setError("Error fetching vehicle type data");
            console.error(error);
        } finally {
            setIsLoading(false);
        }
    };

    const handleEdit = (vehicleType: any) => {
        setVehicleTypeData(vehicleType);
        setIsEditMode(true);
    };

    const handleCancelEdit = () => {
        setVehicleTypeData({
            vehicleTypeId: 0,
            typeName: "",
            isActive: false,
        });
        setIsEditMode(false);
    };

    const paginateData = (data: any[]) => {
        const startIndex = (currentPage - 1) * itemsPerPage;
        return data.slice(startIndex, startIndex + itemsPerPage);
    };

    const handlePageChange = (page: number) => {
        setCurrentPage(page);
    };

    useEffect(() => {
        fetchVehicleTypeData();
    }, []);

    useEffect(() => {
        setCurrentPage(1);  // Reset to page 1 when data changes
    }, [vehicleTypes]);

    const totalPages = Math.ceil(vehicleTypes.length / itemsPerPage);

    return (
        <div className="mt-4 ml-4" style={{ textAlign: 'left', marginLeft: '10px' }}>
            <h3>Vehicle Type Management</h3>

            <Form onSubmit={handleSubmit} className="vehicle-type-form">
                <Row>
                    <Col sm={12} md={6}>
                        <FormGroup>
                            <Label for="typeName">Type Name</Label>
                            <Input
                                type="text"
                                name="typeName"
                                id="typeName"
                                placeholder="Enter type name"
                                value={vehicleTypeData.typeName}
                                onChange={handleChange}
                                required
                            />
                        </FormGroup>
                    </Col>
                    <Col sm={12} md={6}>
                        <FormGroup>
                            <Label>Is Active</Label>
                            <Switch
                                checked={vehicleTypeData.isActive}
                                name="isActive"
                                color="primary"
                                onChange={(e) => setVehicleTypeData({ ...vehicleTypeData, isActive: e.target.checked })}
                                
                            />
                        </FormGroup>
                    </Col>
                </Row>

                <div className="button-group d-flex flex-column flex-sm-row">
                    <Button
                        color="primary"
                        type="submit"
                        className="submit-button"
                        style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}
                    >
                        {isEditMode ? "Update" : "Submit"}
                    </Button>

                    {isEditMode && (
                        <Button
                            color="secondary"
                            type="button"
                            onClick={handleCancelEdit}
                            className="ms-2 action-button"
                            style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}
                        >
                            Cancel
                        </Button>
                    )}
                </div>

            </Form>

            <Row className="mt-4">
                <Col sm={12}>
                    <h4>Vehicle Type Data</h4>
                    {isLoading && <p>Loading...</p>}
                    {error && <p>{error}</p>}
                    {vehicleTypes.length === 0 ? (
                        <p>No vehicle types found.</p>
                    ) : (
                        <Table className="table table-bordered">
                            <thead>
                                <tr>
                                    <th style={{ backgroundColor: '#f8f9fa' }}>Type Name</th>
                                    <th style={{ backgroundColor: '#f8f9fa' }}>Status</th>
                                    <th style={{ backgroundColor: '#f8f9fa' }}>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {paginateData(vehicleTypes).map((vehicleType: any) => (
                                    <tr key={vehicleType.vehicleTypeId}>
                                        <td>{vehicleType.typeName}</td>
                                        <td>{vehicleType.isActive ? 'Active' : 'Inactive'}</td>
                                        <td>
                                            <Button
                                                size="sm"
                                                color="warning"
                                                onClick={() => handleEdit(vehicleType)}
                                            >
                                                Edit
                                            </Button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </Table>
                    )}
                    
                    <div className="pagination-controls">
                        <Button
                            color="secondary"
                            onClick={() => handlePageChange(currentPage - 1)}
                            disabled={currentPage === 1}
                            style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}

                        >
                            Prev
                        </Button>
                        <span>Page {currentPage} of {totalPages}</span>
                        <Button
                            color="secondary"
                            onClick={() => handlePageChange(currentPage + 1)}
                            disabled={currentPage === totalPages}
                            style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}

                        >
                            Next
                        </Button>
                    </div>
                </Col>
            </Row>

            <style jsx>{`
                .form-container {
                    max-width: 100%;
                    margin: 0 auto;
                    padding: 15px;
                    background-color: #f8f9fa;
                    border-radius: 8px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                }
                .button-group {
                    display: flex;
                    flex-direction: column;
                    gap: 10px;
                    margin-top: 20px;
                }
                .vehicle-type-form {
                    margin-bottom: 20px;
                }
                table {
                    margin-top: 20px;
                    width: 100%;
                    border-collapse: collapse;
                }
                .table-responsive {
                    overflow-x: auto;
                }
                
                @media (max-width: 576px) {
                    .table-responsive {
                        -webkit-overflow-scrolling: touch;
                    }
                }
            `}</style>
        </div>
    );
};

export default Page;
