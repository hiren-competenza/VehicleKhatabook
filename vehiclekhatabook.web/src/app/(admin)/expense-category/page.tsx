"use client";
import React, { useState, useEffect } from 'react';
import { Form, FormGroup, Label, Input, Button, Row, Col } from 'reactstrap';
import { addExpenseCategory, getExpenseCategory, updateExpenseCategory , getLanguageType} from '@/service/admin.service';
import { Switch } from '@mui/material';

const Page = () => {
    const [expenseCategoryData, setExpenseCategoryData] = useState({
        expenseCategoryID: 0,
        name: "",
        RoleId: 1,
        description: "",
        isActive: true,
        expenseCategoryLanguageJson:""
    });
    const [isEditMode, setIsEditMode] = useState(false);
    const [expenseCategories, setExpenseCategories] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize] = useState(5); // You can change this value for page size customization
    const [languageData, setLanguageData] = useState([]);
    const [languageInputs, setLanguageInputs] = useState<Record<number, string>>(
      {}
    );
    useEffect(() => {
        fetchExpenseData();
    }, []);

    const fetchExpenseData = async () => {
        try {
            const languages = await getLanguageType();
            const data = await getExpenseCategory();
            setLanguageData(languages);
            setExpenseCategories(data);
        } catch (error) {
            console.error("Error fetching expense categories:", error);
        }
    };

    const handleChange = (e: any) => {
        const { name, value, type, checked } = e.target;
        setExpenseCategoryData((prevData) => ({
            ...prevData,
            [name]: type === "checkbox" ? checked : value,
        }));
    };
    const handleLanguageInputChange = (
        e: React.ChangeEvent<HTMLInputElement>,
        languageTypeId: number
      ) => {
        const { value } = e.target;
        setLanguageInputs((prev) => ({
          ...prev,
          [languageTypeId]: value,
        }));
      };
      const handleGenerateJSON = () => {
        debugger
        const jsonData = languageData.map((language: any) => ({
            languageTypeId: language.languageTypeId,
            languageName: language.languageName,
            TranslatedLanguage: languageInputs[language.languageTypeId] || "",
        }));
        return JSON.stringify(jsonData);
    };
    const handlePaginationChange = (newPage: number) => {
        setCurrentPage(newPage);
    };

    const handleRadioChange = (e: any) => {
        setExpenseCategoryData((prevData) => ({
            ...prevData,
            RoleId: parseInt(e.target.value, 10),
        }));
    };

    const handleSubmit = async (e: any) => {debugger
        e.preventDefault();
        console.log("Submitting data:", expenseCategoryData); // Log data being sent
        try {
            if (isEditMode) {
                await updateExpenseCategory(expenseCategoryData);
            } else {
                await addExpenseCategory(expenseCategoryData); // This is where the 500 error occurs
            }
            setIsEditMode(false);
            setExpenseCategoryData({
                expenseCategoryID: 0,
                RoleId: 1,
                name: "",
                description: "",
                isActive: false,
                expenseCategoryLanguageJson:""

            });
            fetchExpenseData();
            handleGenerateJSON();

        } catch (error) {
            console.error("Error adding/updating expense category data:", error);
        }
    };

    const handleEdit = (category: any) => {
        setExpenseCategoryData(category);
        setIsEditMode(true);
    };

    const handleCancel = () => {
        setExpenseCategoryData({ expenseCategoryID: 0, RoleId: 1, name: "", description: "", isActive: false, expenseCategoryLanguageJson:""
        });
        setIsEditMode(false);
    };

    const totalPages = Math.ceil(expenseCategories.length / pageSize);

    const currentPageData = expenseCategories.slice(
        (currentPage - 1) * pageSize,
        currentPage * pageSize
    );

    return (
        <div className="mt-4 ml-4" style={{ textAlign: 'left', marginLeft: '10px' }}>
            <h3>Expense Category Management</h3>

            <Form onSubmit={handleSubmit} className="expense-category-form">
                <Row>
                    <Col xs={12} md={6}>
                        <FormGroup>
                            <Label for="name">Name</Label>
                            <Input
                                type="text"
                                name="name"
                                id="name"
                                placeholder="Enter name"
                                value={expenseCategoryData.name}
                                onChange={handleChange}
                                required
                            />
                        </FormGroup>
                    </Col>
                    <Col xs={12} md={6}>
                        <FormGroup>
                            <Label for="description">Description</Label>
                            <Input
                                type="text"
                                name="description"
                                id="description"
                                placeholder="Enter description"
                                value={expenseCategoryData.description}
                                onChange={handleChange}
                                required
                            />
                        </FormGroup>
                    </Col>
                    <Col xs={12} md={6}>
                        <FormGroup>
                            <Label>Is Active</Label>
                            <Switch
                                checked={expenseCategoryData.isActive}
                                name="isActive"
                                color="primary"
                                onChange={handleChange}
                            />
                        </FormGroup>
                    </Col>
                    <Col md={6} sm={12}>
                        <FormGroup>
                            <Label>Role Type</Label>
                            <div>
                                <Label check>
                                    <Input
                                        type="radio"
                                        name="Role"
                                        value="1"
                                        checked={expenseCategoryData.RoleId === 1}
                                        onChange={handleRadioChange}
                                    />
                                    Owner
                                </Label>
                                <Label check className="ms-3">
                                    <Input
                                        type="radio"
                                        name="Role"
                                        value="2"
                                        checked={expenseCategoryData.RoleId === 2}
                                        onChange={handleRadioChange}
                                    />
                                    Driver
                                </Label>
                            </div>
                        </FormGroup>
                    </Col>
                </Row>                
            </Form>
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
                            onClick={handleCancel}
                            className="ms-2 action-button"
                            style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}
                        >
                            Cancel
                        </Button>
                    )}
                </div>
            <Row className="mt-4">
                <h4>Expense Category Data</h4>
                <Col md={12}>
                    <div>
                        <table className="table table-bordered">
                            <thead>
                                <tr>
                                    <th>Name</th>
                                    <th>Description</th>
                                    <th>Is Active</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {currentPageData.map((category: any) => (
                                    <tr key={category.ExpenseCategoryID}>
                                        <td>{category.name}</td>
                                        <td>{category.description}</td>
                                        <td>{category.isActive ? 'Yes' : 'No'}</td>
                                        <td>
                                            <Button
                                                size="sm"
                                                color="warning"
                                                onClick={() => handleEdit(category)}
                                            >
                                                Edit
                                            </Button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </Col>
            </Row>

            <Row className="mt-4">
                <Col md={12} className="pagination-controls">
                    <Button
                        color="secondary"
                        onClick={() => handlePaginationChange(currentPage - 1)}
                        disabled={currentPage === 1}
                        style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}
                    >
                        Previous
                    </Button>
                    <span className="mx-3">
                        Page {currentPage} of {totalPages}
                    </span>
                    <Button
                        color="secondary"
                        onClick={() => handlePaginationChange(currentPage + 1)}
                        disabled={currentPage === totalPages}
                        style={{ backgroundColor: '#F3AB3C', borderColor: '#F3AB3C' }}
                    >
                        Next
                    </Button>
                </Col>
            </Row>

            <style jsx>{`
                .form-container {
                    max-width: 100%;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #f8f9fa;
                    border-radius: 8px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                }
                .expense-category-form {
                    display: flex;
                    flex-direction: column;
                    gap: 20px;
                }
                .expense-category-list {
                    margin-top: 20px;
                    background-color: #ffffff;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                }
              .submit-button {
        margin-top: 20px;
        width: 100%;
        max-width: 200px;
        align-self: center;
        background-color: #ff0000; /* Red background */
        border-color: #ff0000;    /* Red border */
        color: white;             /* White text */
        transition: background-color 0.3s, border-color 0.3s;
    }
    .submit-button:hover {
        background-color: #cc0000; /* Slightly darker red on hover */
        border-color: #cc0000;    /* Match hover border color */
    }
                .table {
                    width: 100%;
                    border-collapse: collapse;
                }
                .table th, .table td {
                    text-align: left;
                    padding: 8px;
                }
                .table th {
                    background-color: #f2f2f2;
                }
                @media (max-width: 768px) {
                    .submit-button {
                        width: 100%;
                    }
                    .expense-category-form {
                        flex-direction: column;
                    }
                    .expense-category-list {
                        padding: 15px;
                    }
                }
            `}</style>
        </div>
    );
};

export default Page;
