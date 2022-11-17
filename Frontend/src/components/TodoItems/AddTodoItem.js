import '../../App.css'
import React, { useState, useEffect } from 'react'
import { Button, Container, Row, Col, Form, Stack } from 'react-bootstrap'
import { addTodoItem } from '../../services/TodoItemService'

const AddTodoItem = (props) => {
    const [description, setDescription] = useState('')
    useEffect(() => {}, [])

    async function handleAdd() {
        try {
            addTodoItem({
                description: description
            }).then(() => {
                handleClear()
                props.setShouldRefresh(Math.random())
            })
        } catch (error) {
            console.error(error)
        }
    }
    
    function handleClear() {
        setDescription('')
    }

    const handleDescriptionChange = (event) => {
        setDescription(event.target.value)
    }

    return (
        <Container>
          <h1>Add Item</h1>
          <Form.Group as={Row} className="mb-3" controlId="formAddTodoItem">
            <Form.Label column sm="2">
              Description
            </Form.Label>
            <Col md="6">
              <Form.Control
                type="text"
                placeholder="Enter description..."
                value={description}
                onChange={handleDescriptionChange}
              />
            </Col>
          </Form.Group>
          <Form.Group as={Row} className="mb-3 offset-md-2" controlId="formAddTodoItem">
            <Stack direction="horizontal" gap={2}>
              <Button variant="primary" onClick={() => handleAdd()}>
                Add Item
              </Button>
              <Button variant="secondary" onClick={() => handleClear()}>
                Clear
              </Button>
            </Stack>
          </Form.Group>
        </Container>
      )
}

export default AddTodoItem