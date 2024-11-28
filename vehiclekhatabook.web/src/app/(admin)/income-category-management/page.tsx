"use client";
import React, { useState, useEffect } from "react";
import { Form, FormGroup, Label, Input, Button, Row, Col } from "reactstrap";
import {
    addIncomeCategory,
    getIncomeCategory,
    updateIncomeCategory,
    getLanguageType,
} from "@/service/admin.service";
import Switch from "@mui/material/Switch";

const Page = () => {
    const [incomeCategoryData, setIncomeCategoryData] = useState({
        IncomeCategoryID: 0,
        RoleId: 1,
        name: "",
        isActive: true,
        description: "",
        IncomeCategoryLanguageJson: ""
    });

    const [isEditMode, setIsEditMode] = useState(false);
    const [incomeCategories, setIncomeCategories] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize] = useState(5);
    const [languageData, setLanguageData] = useState([]);
    const [languageInputs, setLanguageInputs] = useState<Record<number, string>>({});
    const [successMessage, setSuccessMessage] = useState(""); // State for success message

    useEffect(() => {
        fetchData();
    }, []);

    const fetchData = async () => {
        try {
            const languages = await getLanguageType();
            const categories = await getIncomeCategory();
            setLanguageData(languages);
            setIncomeCategories(categories);
        } catch (error) {
            console.error("Error fetching data:", error);
        }
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value, type, checked } = e.target;
        setIncomeCategoryData((prevData) => ({
            ...prevData,
            [name]: type === "checkbox" ? checked : value,
        }));
    };

    const handleLanguageInputChange = (e: React.ChangeEvent<HTMLInputElement>, languageName: string) => {
        
        setLanguageInputs({
            ...languageInputs,
            [languageName]: e.target.value, // Update the language input based on the name
        });
    };

    const handleGenerateJSON = () => {
        
        const jsonData = languageData.map((language: any) => ({
            languageTypeId: language.languageTypeId,
            languageName: language.languageName,
            TranslatedLanguage: languageInputs[language.languageTypeId] || "",
        }));
        return JSON.stringify(jsonData);
    };

    const handleRadioChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setIncomeCategoryData((prevData) => ({
            ...prevData,
            RoleId: parseInt(e.target.value, 10),
        }));
    };

    const handleSubmit = async (e: React.FormEvent) => {debugger
        e.preventDefault();

        const incompleteFields = languageData.some(
            (language: any) =>
              !languageInputs[language.languageTypeId]?.trim()
          );
      
          if (incompleteFields) {
            const confirmFillDefault = window.confirm(
              `Some language fields are empty. If you proceed, the default language value will be the "${incomeCategoryData.name}". Do you want to continue?`
            );
      
            if (confirmFillDefault) {
              const updatedLanguageInputs = { ...languageInputs };
              languageData.forEach((language: any) => {
                if (!updatedLanguageInputs[language.languageTypeId]?.trim()) {
                  updatedLanguageInputs[language.languageTypeId] =
                    incomeCategoryData.name;
                }
              });
              setLanguageInputs(updatedLanguageInputs);
              return;
            } else {
              return;
            }
          }
          const generatedJSON = handleGenerateJSON();

          const updatedData = {
            ...incomeCategoryData,
            IncomeCategoryLanguageJson: generatedJSON, // Add the JSON string to the description field
          };
        try {
            if (isEditMode) {
                await updateIncomeCategory(updatedData);
            } else {
                await addIncomeCategory(updatedData);
            }
            setSuccessMessage(isEditMode ? "Income Category Management updated successfully!" : "Income Category Management added successfully!");

            handleCancel(); 
            fetchData();
            setTimeout(() => setSuccessMessage(""), 3000);
            // Reset the form after success
        } catch (error) {
            console.error("Error submitting data:", error);
        }
    };


    const handleEdit = (category: any) => {
        getIncomeCategory();

        setIsEditMode(true);
        setIncomeCategoryData({
            IncomeCategoryID: category.incomeCategoryID ,
            RoleId: category.roleId,
            name: category.name || "",
            isActive: category.isActive || true,
            description: category.description || "",
            IncomeCategoryLanguageJson: category.incomeCategoryLanguageJson || "",
        });

        // Handle language inputs for JSON
        try {
            const parsedJson = JSON.parse(category.incomeCategoryLanguageJson);
            if (Array.isArray(parsedJson)) {
                const updatedLanguageInputs = parsedJson.reduce((acc: any, language: any) => {
                    acc[language.languageTypeId] = language.TranslatedLanguage;
                    return acc;
                }, {});
                setLanguageInputs(updatedLanguageInputs);
            }
        } catch (error) {
            console.error("Error parsing JSON description:", error);
            setLanguageInputs({});
        }
    };



    const handleCancel = () => {
        setIncomeCategoryData({
            IncomeCategoryID: 0,
            RoleId: 1,
            name: "",
            isActive: true,
            description: "",
            IncomeCategoryLanguageJson: ""
        });
        setLanguageInputs({}); // Clear language inputs

        setIsEditMode(false);
    };

    const handlePaginationChange = (newPage: number) => {
        setCurrentPage(newPage);
    };

    const currentCategories = incomeCategories.slice(
        (currentPage - 1) * pageSize,
        currentPage * pageSize
    );

    const totalPages = Math.ceil(incomeCategories.length / pageSize);

    return (
        <div className="mt-4">
            <h3>Income Category Management</h3>
            <Form onSubmit={handleSubmit} className="income-category-form">
                <Row>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="name">Name</Label>
                            <Input
                                type="text"
                                name="name"
                                id="name"
                                placeholder="Enter name"
                                value={incomeCategoryData.name}
                                onChange={handleChange}
                                required
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label for="description">Description</Label>
                            <Input
                                type="text"
                                name="description"
                                id="description"
                                placeholder="Enter description"
                                value={incomeCategoryData.description}
                                onChange={handleChange}
                                required
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label>Is Active</Label>
                            <Switch
                                checked={incomeCategoryData.isActive}
                                name="isActive"
                                color="primary"
                                onChange={(e) =>
                                    setIncomeCategoryData({
                                        ...incomeCategoryData,
                                        isActive: e.target.checked,
                                    })
                                }
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6}>
                        <FormGroup>
                            <Label>Role Type</Label>
                            <div>
                                <Label check>
                                    <Input
                                        type="radio"
                                        name="RoleId"
                                        value="1"
                                        checked={incomeCategoryData.RoleId === 1}
                                        onChange={handleRadioChange}
                                    />

                                    Owner
                                </Label>
                                <Label check className="ms-3">
                                    <Input
                                        type="radio"
                                        name="RoleId"
                                        value="2"
                                        checked={incomeCategoryData.RoleId === 2}
                                        onChange={handleRadioChange}
                                    />
                                    Driver
                                </Label>
                            </div>
                        </FormGroup>
                    </Col>
                </Row>
                <h3>Language Management</h3>
                <div className="language-data">
                    <table className="table table-bordered">
                        <thead>
                            <tr>
                                <th>Language Name</th>
                                <th>Input</th>
                            </tr>
                        </thead>
                        <tbody>
                            {languageData.map((language: any) => (
                                <tr key={language.languageTypeId}>
                                    <td>{language.languageName}</td>
                                    <td>
                                        <Input
                                            type="text"
                                            placeholder="Enter value"
                                            value={languageInputs[language.languageTypeId] || ""}
                                            onChange={(e) =>
                                                handleLanguageInputChange(e, language.languageTypeId)
                                            }
                                        />
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
                {successMessage && <div className="alert alert-success mt-3">{successMessage}</div>}

                <div className="button-group d-flex flex-column flex-sm-row">
                    <Button
                        color="primary"
                        type="submit"
                        style={{ backgroundColor: "#F3AB3C", borderColor: "#F3AB3C" }}
                    >
                            {isEditMode ? 'Update Income Category' : 'Add Income Category'}
                            </Button>

                    {isEditMode && (
                        <Button
                            color="secondary"
                            onClick={handleCancel}
                            className="ms-2"
                            style={{ backgroundColor: "#F3AB3C", borderColor: "#F3AB3C" }}
                        >
                            Cancel
                        </Button>
                    )}
                </div>
            </Form>

            <div>
                <Row className="mt-4">
                    <Col md={12}>
                        <h4>Income Category Data</h4>
                        <table className="table table-bordered">
                            <thead>
                                <tr>
                                    <th>Name</th>
                                    <th>Description</th>
                                    <th>Active</th>
                                    <th>Role Type</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                {currentCategories.length > 0 ? (
                                    currentCategories.map((category: any) => (
                                        <tr key={category.IncomeCategoryID}>
                                            <td>{category.name}</td>
                                            <td>{category.description}</td>
                                            <td>{category.isActive ? "Yes" : "No"}</td>
                                            <td>{category.roleId === 1 ? "Owner" : "Driver"}</td>
                                            <td>
                                                <Button
                                                    onClick={() => handleEdit(category)}
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
                                        <td colSpan={5}>No data available</td>
                                    </tr>
                                )}
                            </tbody>
                        </table>
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

            <style jsx>{`
                .container {
                    max-width: 600px;
                    margin: 0 auto;
                }
                .form-container,
                .income-category-list {
                    background-color: #f8f9fa;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                    margin-bottom: 20px;
                }
                .income-category-form {
                    display: flex;
                    flex-direction: column;
                    gap: 20px;
                }
                .submit-button {
                    width: 100%;
                    background-color: #007bff;
                    border-color: #007bff;
                    color: white;
                    transition: background-color 0.3s;
                }
                .submit-button:hover {
                    background-color: #0056b3;
                    border-color: #0056b3;
                }
                .table {
                    width: 100%;
                    border-collapse: collapse;
                    overflow-x: auto;
                }
                .table th,
                .table td {
                    padding: 8px;
                    text-align: left;
                    border-bottom: 1px solid #ddd;
                }
                .table th {
                    background-color: #f8f9fa;
                }
            `}</style>
        </div>
    );
};

export default Page;
