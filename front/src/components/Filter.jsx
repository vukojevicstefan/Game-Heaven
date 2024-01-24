// Filter.js

import React, { useState } from 'react';
import '../css/Filter.css'
const Filter = ({ onFilterChange }) => {
  const [filterCriteria, setFilterCriteria] = useState({
    title: '', // Change from 'name' to 'title'
    minRating: 0,
    platform: '',
    genre: '',
  });

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFilterCriteria({ ...filterCriteria, [name]: value });
  };


  const handleFilterClick = () => {
    onFilterChange(filterCriteria);
  };

  return (
    <div  className='filter-container'>
      <label>
        Title:
        <input type="text" name="title" value={filterCriteria.title} onChange={handleInputChange} />
      </label>
      <label>
        Min Rating:
        <input
          type="number"
          name="minRating"
          value={filterCriteria.minRating}
          onChange={handleInputChange}
        />
      </label>
      <label>
        Platform:
        <input
          type="text"
          name="platform"
          value={filterCriteria.platform}
          onChange={handleInputChange}
        />
      </label>
      <label>
        Genre:
        <input type="text" name="genre" value={filterCriteria.genre} onChange={handleInputChange} />
      </label>
      <button name='Apply' onClick={handleFilterClick}>Apply Filter</button>
    </div>
  );
};

export default Filter;
