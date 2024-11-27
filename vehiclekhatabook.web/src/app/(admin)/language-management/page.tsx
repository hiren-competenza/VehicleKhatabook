"use client";
import React, { useState, useEffect } from 'react';
import { Form, FormGroup, Label, Input, Button, Row, Col } from 'reactstrap';
import { Switch } from '@mui/material';
import { addLanguageType, updateLanguageType, getLanguageType } from '@/service/admin.service';

const Page = () => {
    const [languageData, setLanguageData] = useState({
        isActive: true,
        description: "",
        languageName: "",
        languageTypeId: 0,
    });
    const [languages, setLanguages] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState("");
    const [isEditMode, setIsEditMode] = useState(false);
    const [currentPage, setCurrentPage] = useState(1);
    const [languagesPerPage] = useState(5); // You can change this to adjust how many rows per page
    const [successMessage, setSuccessMessage] = useState(""); // State for success message

    const handleChange = (e: any) => {
        const { name, value, type, checked } = e.target;
        setLanguageData((prevData) => ({
            ...prevData,
            [name]: type === "checkbox" ? checked : value,
        }));
    };

    const handleSubmit = async (e: any) => {
        e.preventDefault();
        try {
            if (isEditMode) {
                await updateLanguageType(languageData);
            } else {
                await addLanguageType(languageData);
            }
            setSuccessMessage(isEditMode ? "Language Type Management updated successfully!" : "Language Type Management added successfully!");
            setIsEditMode(false);
            setLanguageData({
                isActive: true,
                description: "",
                languageName: "",
                languageTypeId: 0,
            });
            
            fetchLanguageData();
            setTimeout(() => setSuccessMessage(""), 3000);

        } catch (error) {
            console.error("Error saving language data:", error);
        }
    };

    const fetchLanguageData = async () => {
        setIsLoading(true);
        setError("");
        try {
            const data = await getLanguageType();
            setLanguages(data);
        } catch (error) {
            setError("Error fetching language data");
            console.error(error);
        } finally {
            setIsLoading(false);
        }
    };

    const handleEdit = (language: any) => {
        setLanguageData(language);
        setIsEditMode(true);
    };

    const handleCancel = () => {
        setIsEditMode(false);
        setLanguageData({
            isActive: false,
            description: "",
            languageName: "",
            languageTypeId: 0,
        });
    };

    useEffect(() => {
        fetchLanguageData();
    }, []);

    // Logic for pagination
    const indexOfLastLanguage = currentPage * languagesPerPage;
    const indexOfFirstLanguage = indexOfLastLanguage - languagesPerPage;
    const currentLanguages = languages.slice(indexOfFirstLanguage, indexOfLastLanguage);

    const paginate = (pageNumber: number) => setCurrentPage(pageNumber);

    const totalPages = Math.ceil(languages.length / languagesPerPage);

    return (
        <div className="mt-4 ml-4" style={{ textAlign: 'left', marginLeft: '10px' }}>
            <h3>Language Type Management</h3>
            <Form onSubmit={handleSubmit}>
                <Row>
                    <Col md={6} xs={12}>
                        <FormGroup>
                            <Label for="languageName">Language Name</Label>
                            <Input
                                type="text"
                                name="languageName"
                                id="languageName"
                                placeholder="Enter language name"
                                value={languageData.languageName}
                                onChange={handleChange}
                                required
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6} xs={12}>
                        <FormGroup>
                            <Label for="description">Description</Label>
                            <Input
                                type="textarea"
                                name="description"
                                id="description"
                                placeholder="Enter description"
                                value={languageData.description}
                                onChange={handleChange}
                                required
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6} xs={12}>
                        <FormGroup>
                            <Label>Is Active</Label>
                            <Switch
                                checked={languageData.isActive}
                                name="isActive"
                                color="primary"
                                onChange={(e) => setLanguageData({ ...languageData, isActive: e.target.checked })}
                            />
                        </FormGroup>
                    </Col>
                </Row>
                {successMessage && <div className="alert alert-success mt-3">{successMessage}</div>}
                <div className="button-group d-flex flex-column flex-sm-row">
                    <Button
                        color="primary"
                        type="submit"
                        className="submit-button"
                        style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}
                    >
                            {isEditMode ? 'Update Language Type' : 'Add Language Type'}
                            </Button>

                    {isEditMode && (
                        <Button
                            color="secondary"
                            type="button"
                            onClick={handleCancel}
                            className="ms-2 action-button"
                            style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}
                        >
                            Cancel
                        </Button>
                    )}
                </div>

            </Form>

            <Row className="mt-4">
                <Col md={12}>
                    <h4>Language Type Data</h4>
                    {isLoading && <p>Loading...</p>}
                    {error && <p>{error}</p>}
                    <table className="table table-bordered">
                        <thead>
                            <tr>
                                <th style={{ backgroundColor: '#f8f9fa' }}>Language Name</th>
                                <th style={{ backgroundColor: '#f8f9fa' }}>Description</th>
                                <th style={{ backgroundColor: '#f8f9fa' }}>Status</th>
                                <th style={{ backgroundColor: '#f8f9fa' }}>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {currentLanguages.length === 0 ? (
                                <tr>
                                    <td colSpan={4}>No languages found.</td>
                                </tr>
                            ) : (
                                currentLanguages.map((lang: any) => (
                                    <tr key={lang.languageTypeId}>
                                        <td>{lang.languageName}</td>
                                        <td>{lang.description}</td>
                                        <td>{lang.isActive ? "Active" : "Inactive"}</td>
                                        <td>
                                            <Button size="sm" color="warning" onClick={() => handleEdit(lang)}>
                                                Edit
                                            </Button>
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                    <div className="pagination-controls">
                        <Button
                            color="secondary"
                            onClick={() => paginate(currentPage - 1)}
                            disabled={currentPage === 1}
                            style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}

                        >
                            Prev
                        </Button>
                        <span>Page {currentPage} of {totalPages}</span>
                        <Button
                            color="secondary"
                            onClick={() => paginate(currentPage + 1)}
                            disabled={currentPage === totalPages}
                            style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}

                        >
                            Next
                        </Button>
                    </div>
                </Col>

            </Row>

            <style jsx>{`
                .container {
                    max-width: 1000px;
                    margin: 0 auto;
                    padding: 20px;
                    border-radius: 8px;
                    text-align: left;
                }
                .button-row {
                    margin-top: 15px;
                    display: flex;
                    gap: 10px;
                    justify-content: flex-start;
                }
                .action-button {
                    padding: 8px 16px;
                    font-size: 14px;
                    text-align: center;
                    white-space: nowrap;
                }
                .submit-button {
                    width: 120px;
                    padding: 8px 20px;
                    font-size: 16px;
                    background-color: #007bff;
                    border-color: #007bff;
                    color: white;
                    align-self: center;
                    margin-top: 15px;
                    transition: background-color 0.3s;
                }
                .submit-button:hover {
                    background-color: #0056b3;
                    border-color: #0056b3;
                }
                .table {
                    margin-top: 20px;
                    width: 100%;
                    background-color: #fff;
                }
                .pagination {
                    margin-top: 10px;
                    text-align: center;
                }
                .pagination Button {
                    margin: 0 5px;
                }
            `}</style>
        </div>
    );
};

export default Page;
