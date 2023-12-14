export default {

  format: function (format_type, data) {
    if (format_type == 1) {
      return this.___convert_to_common_format(data);
    } else if (format_type == 2) {
      return this.___convert_to_space_format(data);
    } else if (format_type == 3) {
      return this.___convert_to_number_format(data);
    } else {
      return 'Invalid format type';
    }

  },

  /**
   * also known as format type 1
   * @param {string} data
   * @return {string}
   */
  ___convert_to_common_format: function(data) {
    //replace numbers and spaces with empty string
    return data.replace(/[^a-zA-Z]/g, '');
  },


  /**
   * This is known as format type 2
   * @param {string} data
   * @return {string}
   */
  ___convert_to_space_format: function(data) {
    const new_data = this.___convert_to_common_format(data)
    return new_data.replace(/(.{10})/g, '$1 ').replace(/(.{66})/g, '$1\n');
  },

  /**
   * This is known as format type 3
   * @param {string} data
   * @return {string}
   */
  ___convert_to_number_format: function(data) {
    //use the ___convert_to_space_format but prepend with a a number that starts with 1 and increments by 60 for each line
    let space_format = this.___convert_to_space_format(data);
    let space_format_lines = space_format.split('\n');
    let number_format = '';
    for (let i = 0; i < space_format_lines.length; i++) {
      number_format += (i * 60 + 1) + ' ' + space_format_lines[i] + '\n';
    }
    return number_format;
  }

}
